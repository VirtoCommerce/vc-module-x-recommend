using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Data.GenericCrud;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Services;
using VirtoCommerce.XRecommend.Data.Models;
using VirtoCommerce.XRecommend.Data.Repositories;

namespace VirtoCommerce.XRecommend.Data.Services;

public class UserEventSearchService : SearchService<UserEventSearchCriteria, UserEventSearchResult, UserEvent, UserEventEntity>, IUserEventSearchService
{
    public UserEventSearchService(
        Func<IRecommendRepository> repositoryFactory,
        IPlatformMemoryCache platformMemoryCache,
        IUserEventService crudService,
        IOptions<CrudOptions> crudOptions)
        : base(repositoryFactory, platformMemoryCache, crudService, crudOptions)
    {
    }

    protected override IQueryable<UserEventEntity> BuildQuery(IRepository repository, UserEventSearchCriteria criteria)
    {
        var query = ((IRecommendRepository)repository).UserEvents;

        if (!string.IsNullOrEmpty(criteria.UserId))
        {
            query = query.Where(x => x.UserId == criteria.UserId);
        }

        if (!string.IsNullOrEmpty(criteria.ProductId))
        {
            query = query.Where(x => x.ProductId == criteria.ProductId);
        }

        return query;
    }


    protected override IList<SortInfo> BuildSortExpression(UserEventSearchCriteria criteria)
    {
        var sortInfos = criteria.SortInfos;

        if (sortInfos.IsNullOrEmpty())
        {
            sortInfos =
            [
                new SortInfo { SortColumn = nameof(UserEvent.CreatedDate), SortDirection = SortDirection.Descending },
            ];
        }

        return sortInfos;
    }
}
