using System;
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

public class SaveSearchQueryCommandBuilder(IAuthorizationService authorizationService)
    : CommandBuilder<SaveSearchQueryCommand, bool, InputSaveSearchQueryType, BooleanGraphType>
        (authorizationService)
{
    protected override string Name => "saveSearchQuery";

    [Obsolete("Use the constructor without IMediator. The mediator is resolved from context.RequestServices per request.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public SaveSearchQueryCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : this(authorizationService)
    {
    }

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, SaveSearchQueryCommand request)
    {
        request.UserId = context.GetCurrentUserId();
        request.OrganizationId = context.GetCurrentOrganizationId();

        await Authorize(context, request, new SearchHistoryAuthorizationRequirement());
        await base.BeforeMediatorSend(context, request);
    }
}
