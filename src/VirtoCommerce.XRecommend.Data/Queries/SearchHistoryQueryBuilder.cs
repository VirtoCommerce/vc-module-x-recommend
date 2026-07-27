using System;
using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Queries;
using VirtoCommerce.XRecommend.Core.Schemas;
using VirtoCommerce.XRecommend.Data.Authorization;

namespace VirtoCommerce.XRecommend.Data.Queries;

public class SearchHistoryQueryBuilder(IAuthorizationService authorizationService)
    : QueryBuilder<SearchHistoryQuery, SearchHistoryResult, SearchHistoryResultType>
        (authorizationService)
{
    protected override string Name => "searchHistory";

    [Obsolete("Use the constructor without IMediator. The mediator is resolved from context.RequestServices per request.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public SearchHistoryQueryBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : this(authorizationService)
    {
    }

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, SearchHistoryQuery request)
    {
        await Authorize(context, request, new SearchHistoryAuthorizationRequirement());
        await base.BeforeMediatorSend(context, request);
    }
}
