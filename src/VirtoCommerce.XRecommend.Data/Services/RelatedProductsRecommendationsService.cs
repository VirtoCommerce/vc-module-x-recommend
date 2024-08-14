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
        var productSearchRequest = GetInputProductSearchRequest(criteria);
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

        // recommended products must be visible
        var recommendedProductsSearchRequest = GetRecommendedProductsSearchRequest(criteria, content, store.Catalog);
        var recommendedProductsSearchResult = await _searchProvider.SearchAsync(KnownDocumentTypes.Product, recommendedProductsSearchRequest);

        result = recommendedProductsSearchResult.Documents.Select(x => x.Id).ToList();

        // optional*: boost semantic query
        // optional**: exclude productfamilyId ('must_not' filter?)

        return result;
    }

    private static SearchRequest GetInputProductSearchRequest(GetRecommendationsCriteria criteria)
    {
        var builder = new IndexSearchRequestBuilder()
            .WithStoreId(criteria.StoreId)
            .WithUserId(criteria.UserId)
            .WithPaging(0, 1)
            .AddObjectIds(new List<string> { criteria.ProductId })
            .WithIncludeFields("code", "__content");

        return builder.Build();
    }

    private static SearchRequest GetRecommendedProductsSearchRequest(GetRecommendationsCriteria criteria, string content, string catalogId)
    {
        var builder = new IndexSearchRequestBuilder()
            .WithStoreId(criteria.StoreId)
            .WithUserId(criteria.UserId)
            .WithPaging(0, criteria.MaxRecommendations)
            .WithSearchPhrase(content)
            .WithIncludeFields("_id");

        builder.AddTerms(new[] { "status:visible" });
        builder.AddTerms(new[] { $"__outline:{catalogId}" });

        return builder.Build();
    }
}
