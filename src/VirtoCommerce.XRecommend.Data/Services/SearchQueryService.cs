using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;
using VirtoCommerce.XRecommend.Core.Events;
using VirtoCommerce.XRecommend.Core.Extensions;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Services;
using VirtoCommerce.XRecommend.Data.Models;
using VirtoCommerce.XRecommend.Data.Repositories;

namespace VirtoCommerce.XRecommend.Data.Services;

public class SearchQueryService(
    Func<IRecommendRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IEventPublisher eventPublisher)
    : CrudService<SearchQuery, SearchQueryEntity, SearchQueryChangingEvent, SearchQueryChangedEvent>
        (repositoryFactory, platformMemoryCache, eventPublisher),
        ISearchQueryService
{
    protected override Task<IList<SearchQueryEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        return ((IRecommendRepository)repository).GetSearchQueriesByIdsAsync(ids, responseGroup);
    }

    // Saving a query for one user should not clear cache for other users
    protected override void ClearSearchCache(IList<SearchQuery> models)
    {
        var customerCacheKeys = models
            .Select(x => x.GetCustomerCacheKey())
            .Distinct();

        foreach (var customerCacheKey in customerCacheKeys)
        {
            GenericSearchCachingRegion<SearchQuery>.ExpireTokenForKey(customerCacheKey);
        }
    }
}
