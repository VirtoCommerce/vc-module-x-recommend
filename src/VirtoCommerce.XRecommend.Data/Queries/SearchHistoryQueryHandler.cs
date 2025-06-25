using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Xapi.Core.Infrastructure;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Queries;
using VirtoCommerce.XRecommend.Core.Services;

namespace VirtoCommerce.XRecommend.Data.Queries;

public class SearchHistoryQueryHandler(ISearchQuerySearchService searchService)
    : IQueryHandler<SearchHistoryQuery, SearchHistoryResult>
{
    public async Task<SearchHistoryResult> Handle(SearchHistoryQuery request, CancellationToken cancellationToken)
    {
        var searchCriteria = AbstractTypeFactory<SearchQuerySearchCriteria>.TryCreateInstance();
        searchCriteria.UserId = request.UserId;
        searchCriteria.StoreId = request.StoreId;
        searchCriteria.OrganizationId = request.OrganizationId;

        var result = AbstractTypeFactory<SearchHistoryResult>.TryCreateInstance();
        result.Queries = new List<string>();

        // Find the requested number of unique queries, but not more than 20
        var maxCount = Math.Min(request.MaxCount, 20);

        await foreach (var searchResult in searchService.SearchBatchesNoCloneAsync(searchCriteria).WithCancellation(cancellationToken))
        {
            foreach (var query in searchResult.Results)
            {
                result.Queries.AddDistinct(StringComparer.OrdinalIgnoreCase, query.Query);

                if (result.Queries.Count >= maxCount)
                {
                    return result;
                }
            }
        }

        return result;
    }
}
