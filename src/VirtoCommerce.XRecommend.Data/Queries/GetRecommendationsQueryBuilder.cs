using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.StoreModule.Core.Services;
using VirtoCommerce.XCatalog.Data.Queries;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Queries;
using VirtoCommerce.XRecommend.Core.Schemas;

namespace VirtoCommerce.XRecommend.Data.Queries;

public class GetRecommendationsQueryBuilder : CatalogQueryBuilder<GetRecommendationsQuery, GetRecommendationsResult, GetRecommendationsResponseType>
{
    public GetRecommendationsQueryBuilder(IMediator mediator, IAuthorizationService authorizationService, IStoreService storeService, ICurrencyService currencyService)
        : base(mediator, authorizationService, storeService, currencyService)
    {
    }

    protected override string Name => "recommendations";
}
