using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.SearchModule.Core.Extensions;
using VirtoCommerce.SearchModule.Core.Model;
using VirtoCommerce.SearchModule.Core.Services;
using VirtoCommerce.StoreModule.Core.Services;
using VirtoCommerce.XCatalog.Data.Index;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Services;

namespace VirtoCommerce.XRecommend.Data.Services;

public class RelatedProductsRecommendationsService : IRecommendationsService
{
    private readonly IStoreService _storeService;
    private readonly ISearchProvider _searchProvider;
    private readonly IConfiguration _configuration;

    public string Model { get; set; } = "related-products";

    private static readonly string[] StatusVisible = ["status:visible"];

    public RelatedProductsRecommendationsService(IStoreService storeService, ISearchProvider searchProvider, IConfiguration configuration)
    {
        _searchProvider = searchProvider;
        _storeService = storeService;
        _configuration = configuration;
    }

    public async Task<IList<string>> GetRecommendationsAsync(GetRecommendationsCriteria criteria)
    {
        // check ES8 enabled and return mock result if not (temporary)
        if (!_configuration.SearchProviderActive("ElasticSearch8"))
        {
            var builder = new IndexSearchRequestBuilder()
                .WithStoreId(criteria.StoreId)
                .WithUserId(criteria.UserId)
                .WithPaging(0, criteria.MaxRecommendations)
                .WithIncludeFields("_id");

            var mockResult = await _searchProvider.SearchAsync(KnownDocumentTypes.Product, builder.Build());
            return mockResult.Documents.Select(x => x.Id).ToList();
        }

        var result = new List<string>();

        var store = await _storeService.GetNoCloneAsync(criteria.StoreId);
        if (store == null)
        {
            return result;
        }

        // get input product
        var productSearchRequest = GetInputProductSearchRequest(criteria, criteria.ProductId);
        var productSearchResult = await _searchProvider.SearchAsync(KnownDocumentTypes.Product, productSearchRequest);

        // take content and form recommendations search request
        var productDocument = productSearchResult.Documents.FirstOrDefault();
        if (productDocument == null || !productDocument.TryGetValue("__content", out var contentObj) || contentObj is not string content)
        {
            return result;
        }

        // main product and variations (if exist) will probably be captured in the semantic query
        // try to remove them by setting take = MaxRecommendations + variations count + 1 (main product) and removing them from the result
        // until we can use 'must_not' filter in the search request
        var excludedProductIds = await GetExcludedProductIds(productDocument, criteria);

        // recommended products must be visible
        var recommendedProductsSearchRequest = GetRecommendedProductsSearchRequest(criteria, content, store.Catalog, criteria.MaxRecommendations + excludedProductIds.Count);
        var recommendedProductsSearchResult = await _searchProvider.SearchAsync(KnownDocumentTypes.Product, recommendedProductsSearchRequest);

        result = recommendedProductsSearchResult.Documents.Select(x => x.Id).ToList();

        foreach (var excludedProductId in excludedProductIds)
        {
            result.Remove(excludedProductId);
        }

        result = result.Take(criteria.MaxRecommendations).ToList();

        return result;
    }

    private async Task<List<string>> GetExcludedProductIds(SearchDocument productDocument, GetRecommendationsCriteria criteria)
    {
        var excludedProductIds = new List<string> { productDocument.Id };
        excludedProductIds.AddRange(GetVariations(productDocument));

        if (productDocument.TryGetValue("mainproductid", out var mainProductIdObj) && mainProductIdObj is string mainProductId && !string.IsNullOrEmpty(mainProductId))
        {
            // take main product and it's variations
            var productSearchRequest = GetInputProductSearchRequest(criteria, mainProductId);
            var productSearchResult = await _searchProvider.SearchAsync(KnownDocumentTypes.Product, productSearchRequest);
            var mainProductDocument = productSearchResult.Documents.FirstOrDefault();

            if (mainProductDocument != null)
            {
                excludedProductIds.Add(mainProductDocument.Id);
                excludedProductIds.AddRange(GetVariations(mainProductDocument));
            }
        }

        return excludedProductIds;
    }

    private static IEnumerable<string> GetVariations(SearchDocument productDocument)
    {
        if (productDocument.TryGetValue("__variations", out var variationsObj) && variationsObj is object[] variationIds)
        {
            return variationIds.OfType<string>();
        }

        return Enumerable.Empty<string>();
    }

    private static SearchRequest GetInputProductSearchRequest(GetRecommendationsCriteria criteria, string prodcutId)
    {
        var builder = new IndexSearchRequestBuilder()
            .WithStoreId(criteria.StoreId)
            .WithUserId(criteria.UserId)
            .WithPaging(0, 1)
            .AddObjectIds(new List<string> { prodcutId })
            .WithIncludeFields("code", "__content", "__variations", "mainproductid");

        return builder.Build();
    }

    private static SearchRequest GetRecommendedProductsSearchRequest(GetRecommendationsCriteria criteria, string content, string catalogId, int take)
    {
        var builder = new IndexSearchRequestBuilder()
            .WithStoreId(criteria.StoreId)
            .WithUserId(criteria.UserId)
            .WithPaging(0, take)
            .WithSearchPhrase(content)
            .WithIncludeFields("_id");

        builder.AddTerms(StatusVisible);
        builder.AddTerms(new[] { $"__outline:{catalogId}" });

        return builder.Build();
    }
}
