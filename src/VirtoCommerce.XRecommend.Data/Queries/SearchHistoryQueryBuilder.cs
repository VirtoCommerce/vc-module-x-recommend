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

public class SearchHistoryQueryBuilder(
    IMediator mediator,
    IAuthorizationService authorizationService)
    : QueryBuilder<SearchHistoryQuery, SearchHistoryResult, SearchHistoryResultType>
        (mediator, authorizationService)
{
    protected override string Name => "searchHistory";

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, SearchHistoryQuery request)
    {
        await Authorize(context, request, new SearchHistoryAuthorizationRequirement());
        await base.BeforeMediatorSend(context, request);
    }
}
