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

public class SaveSearchQueryCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
    : CommandBuilder<SaveSearchQueryCommand, bool, InputSaveSearchQueryType, BooleanGraphType>
        (mediator, authorizationService)
{
    protected override string Name => "saveSearchQuery";

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, SaveSearchQueryCommand request)
    {
        request.UserId = context.GetCurrentUserId();
        request.OrganizationId = context.GetCurrentOrganizationId();

        await Authorize(context, request, new SearchHistoryAuthorizationRequirement());
        await base.BeforeMediatorSend(context, request);
    }
}
