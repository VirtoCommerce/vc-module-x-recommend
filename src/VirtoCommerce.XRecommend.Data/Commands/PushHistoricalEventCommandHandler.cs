using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.XRecommend.Core.Commands;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Services;

namespace VirtoCommerce.XRecommend.Data.Commands;

public class PushHistoricalEventCommandHandler : IRequestHandler<PushHistoricalEventCommand, bool>
{
    private readonly IHistoricalEventService _eventService;
    private readonly IHistoricalEventSearchService _eventSearchService;

    public PushHistoricalEventCommandHandler(IHistoricalEventService eventService, IHistoricalEventSearchService eventSearchService)
    {
        _eventService = eventService;
        _eventSearchService = eventSearchService;
    }

    public async Task<bool> Handle(PushHistoricalEventCommand request, CancellationToken cancellationToken)
    {
        var searchCriteria = new HistoricalEventSearchCriteria
        {
            ProductId = request.ProductId,
            UserId = request.UserId,
            StoreId = request.StoreId,
            EventType = request.EventType,
        };

        var eventSearchResult = await _eventSearchService.SearchAsync(searchCriteria);

        var eventsToSave = new List<HistoricalEvent>();

        if (eventSearchResult.Results.Count > 0)
        {
            foreach (var trackedEvent in eventSearchResult.Results)
            {
                trackedEvent.ModifiedDate = DateTime.UtcNow;
                eventsToSave.Add(trackedEvent);
            }
        }
        else
        {
            var newEvent = AbstractTypeFactory<HistoricalEvent>.TryCreateInstance();
            newEvent.ProductId = request.ProductId;
            newEvent.UserId = request.UserId;
            newEvent.StoreId = request.StoreId;
            newEvent.EventType = request.EventType;

            eventsToSave.Add(newEvent);
        }

        await _eventService.SaveChangesAsync(eventsToSave);

        return true;
    }
}
