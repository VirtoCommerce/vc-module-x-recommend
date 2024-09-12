using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;
using VirtoCommerce.XRecommend.Core.Events;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Services;
using VirtoCommerce.XRecommend.Data.Models;
using VirtoCommerce.XRecommend.Data.Repositories;

namespace VirtoCommerce.XRecommend.Data.Services;

public class HistoricalEventService : CrudService<HistoricalEvent, HistoricalEventEntity, HistoricalEventChangingEvent, HistoricalEventChangedEvent>, IHistoricalEventService
{
    public HistoricalEventService(
        Func<IRecommendRepository> repositoryFactory,
        IPlatformMemoryCache platformMemoryCache,
        IEventPublisher eventPublisher)
        : base(repositoryFactory, platformMemoryCache, eventPublisher)
    {
    }

    protected override Task<IList<HistoricalEventEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        return ((IRecommendRepository)repository).GetHistoricalEventsByIdsAsync(ids, responseGroup);
    }
}
