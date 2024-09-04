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

public class PushEventCommandHandler : IRequestHandler<PushEventCommand, bool>
{
    private readonly IEventService _eventService;
    private readonly IEventSearchService _eventSearchService;

    public PushEventCommandHandler(IEventService eventService, IEventSearchService eventSearchService)
    {
        _eventService = eventService;
        _eventSearchService = eventSearchService;
    }

    public async Task<bool> Handle(PushEventCommand request, CancellationToken cancellationToken)
    {
        var searchCriteria = new EventSearchCriteria
        {
            ProductId = request.ProductId,
            UserId = request.UserId,
            StoreId = request.StoreId,
            EventType = request.EventType,
        };

        var eventSearchResult = await _eventSearchService.SearchAsync(searchCriteria);

        var eventsToSave = new List<Event>();

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
            var newEvent = AbstractTypeFactory<Event>.TryCreateInstance();
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

