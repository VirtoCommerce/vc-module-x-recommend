using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Data.GenericCrud;
using VirtoCommerce.XRecommend.Core.Extensions;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Services;
using VirtoCommerce.XRecommend.Data.Models;
using VirtoCommerce.XRecommend.Data.Repositories;

namespace VirtoCommerce.XRecommend.Data.Services;

public class SearchQuerySearchService(
    Func<IRecommendRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    ISearchQueryService crudService,
    IOptions<CrudOptions> crudOptions)
    : SearchService<SearchQuerySearchCriteria, SearchQuerySearchResult, SearchQuery, SearchQueryEntity>
        (repositoryFactory, platformMemoryCache, crudService, crudOptions),
        ISearchQuerySearchService
{
    protected override IQueryable<SearchQueryEntity> BuildQuery(IRepository repository, SearchQuerySearchCriteria criteria)
    {
        var query = ((IRecommendRepository)repository).SearchQueries;

        if (!string.IsNullOrEmpty(criteria.UserId))
        {
            query = query.Where(x => x.UserId == criteria.UserId);
        }

        if (!string.IsNullOrEmpty(criteria.OrganizationId))
        {
            query = query.Where(x => x.OrganizationId == criteria.OrganizationId);
        }

        if (!string.IsNullOrEmpty(criteria.StoreId))
        {
            query = query.Where(x => x.StoreId == criteria.StoreId);
        }

        if (!string.IsNullOrEmpty(criteria.Keyword))
        {
            query = query.Where(x => x.Query.Contains(criteria.Keyword));
        }

        return query;
    }

    protected override IList<SortInfo> BuildSortExpression(SearchQuerySearchCriteria criteria)
    {
        var sortInfos = criteria.SortInfos;

        if (sortInfos.IsNullOrEmpty())
        {
            sortInfos =
            [
                new SortInfo { SortColumn = nameof(SearchQueryEntity.CreatedDate), SortDirection = SortDirection.Descending },
                new SortInfo { SortColumn = nameof(SearchQueryEntity.Id) },
            ];
        }

        return sortInfos;
    }

    protected override IChangeToken CreateCacheToken(SearchQuerySearchCriteria criteria)
    {
        var customerCacheKey = criteria.GetCustomerCacheKey();

        return GenericSearchCachingRegion<SearchQuery>.CreateChangeTokenForKey(customerCacheKey);
    }
}
