using System;
using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.StoreModule.Core.Services;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Queries;
using VirtoCommerce.XRecommend.Core.Schemas;
using VirtoCommerce.XRecommend.Data.Authorization;

namespace VirtoCommerce.XRecommend.Data.Queries;

public class GetRecentlyBrowsedQueryBuilder : CatalogQueryBuilder<GetRecentlyBrowsedQuery, GetRecentlyBrowsedResult, GetRecentlyBrowsedResponseType>
{
    public GetRecentlyBrowsedQueryBuilder(
        IAuthorizationService authorizationService,
        IStoreService storeService,
        ICurrencyService currencyService)
        : base(authorizationService, storeService, currencyService)
    {
    }

    [Obsolete("Use the constructor without IMediator. The mediator is resolved from context.RequestServices per request.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public GetRecentlyBrowsedQueryBuilder(
        IMediator mediator,
        IAuthorizationService authorizationService,
        IStoreService storeService,
        ICurrencyService currencyService)
        : this(authorizationService, storeService, currencyService)
    {
    }

    protected override string Name => "recentlyBrowsed";

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, GetRecentlyBrowsedQuery request)
    {
        await Authorize(context, null, new RecommendationsAuthorizationRequirement());

        await base.BeforeMediatorSend(context, request);
    }
}
