using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.StoreModule.Core.Services;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Queries;
using VirtoCommerce.XRecommend.Core.Schemas;

namespace VirtoCommerce.XRecommend.Data.Queries;

public class GetRecommendationsQueryBuilder : CatalogQueryBuilder<GetRecommendationsQuery, GetRecommendationsResult, GetRecommendationsResponseType>
{
    public GetRecommendationsQueryBuilder(IAuthorizationService authorizationService, IStoreService storeService, ICurrencyService currencyService)
        : base(authorizationService, storeService, currencyService)
    {
    }

    [Obsolete("Use the constructor without IMediator. The mediator is resolved from context.RequestServices per request.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public GetRecommendationsQueryBuilder(IMediator mediator, IAuthorizationService authorizationService, IStoreService storeService, ICurrencyService currencyService)
        : this(authorizationService, storeService, currencyService)
    {
    }

    protected override string Name => "recommendations";
}
