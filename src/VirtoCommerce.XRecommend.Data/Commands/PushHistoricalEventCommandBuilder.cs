using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.Xapi.Core.Extensions;
using VirtoCommerce.XRecommend.Core.Commands;
using VirtoCommerce.XRecommend.Core.Schemas;
using VirtoCommerce.XRecommend.Data.Authorization;

namespace VirtoCommerce.XRecommend.Data.Commands;

public class PushHistoricalEventCommandBuilder : CommandBuilder<PushHistoricalEventCommand, bool, InputPushHistoricalEventType, BooleanGraphType>
{
    protected override string Name => "pushHistoricalEvent";

    public PushHistoricalEventCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, PushHistoricalEventCommand request)
    {
        await Authorize(context, null, new RecommendationsAuthorizationRequirement());

        request.UserId = context.GetCurrentUserId();

        request.ProductIds ??= [];
        if (!string.IsNullOrEmpty(request.ProductId))
        {
            request.ProductIds.Add(request.ProductId);
            request.ProductIds = request.ProductIds.Distinct().ToList();
        }

        await base.BeforeMediatorSend(context, request);
    }
}
