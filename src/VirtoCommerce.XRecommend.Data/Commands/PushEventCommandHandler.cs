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
            UserId = request.UserId,
            ProductId = request.ProductId,
            EventType = request.EventType,
        };

        var eventSearchResult = await _eventSearchService.SearchAsync(searchCriteria);

        var eventsToSave = new List<UserEvent>();

        if (eventSearchResult.Results.Count > 0)
        {
            foreach (var userEvent in eventSearchResult.Results)
            {
                userEvent.ModifiedDate = DateTime.UtcNow;
                eventsToSave.Add(userEvent);
            }
        }
        else
        {
            var newEvent = AbstractTypeFactory<UserEvent>.TryCreateInstance();
            newEvent.ProductId = request.ProductId;
            newEvent.UserId = request.UserId;
            newEvent.EventType = request.EventType;

            eventsToSave.Add(newEvent);
        }

        await _eventService.SaveChangesAsync(eventsToSave);

        return true;
    }
}

