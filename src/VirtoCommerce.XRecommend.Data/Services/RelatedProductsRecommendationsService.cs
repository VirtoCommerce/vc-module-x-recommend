using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
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

    public RelatedProductsRecommendationsService(IStoreService storeService, ISearchProvider searchProvider)
    {
        _searchProvider = searchProvider;
        _storeService = storeService;
    }

    public async Task<IList<string>> GetRecommendationsAsync(GetRecommendationsCriteria criteria)
    {
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

        // try exclude code from content string
        if (productDocument.TryGetValue("code", out var codeObj) && codeObj is string code)
        {
            content = content.Replace($"{code}.", string.Empty);
        }

        // main product and variations (if exist) will probably be captured in the semantic query
        // try to remove them by setting take = MaxRecommendations + variations count + 1 (main product) and removing them from the result
        // until we can use 'must_not' filter in the search request
        var excludedProductIds = new List<string> { criteria.ProductId };
        if (productDocument.TryGetValue("__variations", out var variationsObj) && variationsObj is object[] variationIds)
        {
            foreach (var variationId in variationIds.OfType<string>())
            {
                excludedProductIds.Add(variationId);
            }
        }

        if (productDocument.TryGetValue("mainproductid", out var mainProductIdObj) && mainProductIdObj is string mainProductId && !string.IsNullOrEmpty(mainProductId))
        {
            // take main product and it's variations
            productSearchRequest = GetInputProductSearchRequest(criteria, mainProductId);
            productSearchResult = await _searchProvider.SearchAsync(KnownDocumentTypes.Product, productSearchRequest);
            productDocument = productSearchResult.Documents.FirstOrDefault();

            if (productDocument != null)
            {
                excludedProductIds.Add(mainProductId);

                if (productDocument.TryGetValue("__variations", out variationsObj) && variationsObj is object[] mainVariationIds)
                {
                    foreach (var variationId in mainVariationIds.OfType<string>())
                    {
                        excludedProductIds.Add(variationId);
                    }
                }
            }

        }

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

        builder.AddTerms(new[] { "status:visible" });
        builder.AddTerms(new[] { $"__outline:{catalogId}" });

        return builder.Build();
    }
}
