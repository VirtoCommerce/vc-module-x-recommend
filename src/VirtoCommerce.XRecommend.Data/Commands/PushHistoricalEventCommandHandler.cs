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
        var eventsToSave = new List<HistoricalEvent>();

        foreach (var productId in request.ProductIds)
        {
            var searchCriteria = AbstractTypeFactory<HistoricalEventSearchCriteria>.TryCreateInstance();
            searchCriteria.ProductId = productId;
            searchCriteria.UserId = request.UserId;
            searchCriteria.StoreId = request.StoreId;
            searchCriteria.SessionId = request.SessionId;
            searchCriteria.EventType = request.EventType;

            var searchResult = await _eventSearchService.SearchAsync(searchCriteria);

            if (searchResult.Results.Count > 0)
            {
                foreach (var trackedEvent in searchResult.Results)
                {
                    trackedEvent.ModifiedDate = DateTime.UtcNow;
                    eventsToSave.Add(trackedEvent);
                }
            }
            else
            {
                var newEvent = AbstractTypeFactory<HistoricalEvent>.TryCreateInstance();
                newEvent.ProductId = productId;
                newEvent.UserId = request.UserId;
                newEvent.StoreId = request.StoreId;
                newEvent.SessionId = request.SessionId;
                newEvent.EventType = request.EventType;

                eventsToSave.Add(newEvent);
            }
        }

        await _eventService.SaveChangesAsync(eventsToSave);

        return true;
    }
}
