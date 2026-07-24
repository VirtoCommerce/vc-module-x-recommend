using System;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.StoreModule.Core.Services;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.Xapi.Core.Extensions;
using VirtoCommerce.XCatalog.Core.Queries;

namespace VirtoCommerce.XRecommend.Data.Queries;

public abstract class CatalogQueryBuilder<TQuery, TResult, TResultGraphType>
    : QueryBuilder<TQuery, TResult, TResultGraphType>
    where TQuery : CatalogQueryBase<TResult>
    where TResultGraphType : IGraphType
{
    private readonly IStoreService _storeService;
    private readonly ICurrencyService _currencyService;

    protected CatalogQueryBuilder(
        IAuthorizationService authorizationService,
        IStoreService storeService,
        ICurrencyService currencyService)
        : base(authorizationService)
    {
        _storeService = storeService;
        _currencyService = currencyService;
    }

    [Obsolete("Use the constructor without IMediator. The mediator is resolved from context.RequestServices per request.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    protected CatalogQueryBuilder(
        IMediator mediator,
        IAuthorizationService authorizationService,
        IStoreService storeService,
        ICurrencyService currencyService)
        : this(authorizationService, storeService, currencyService)
    {
    }

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, TQuery request)
    {
        await base.BeforeMediatorSend(context, request);

        request.IncludeFields = context.SubFields?.Values.GetAllNodesPaths(context).ToArray() ?? Array.Empty<string>();

        if (!string.IsNullOrEmpty(request.StoreId))
        {
            var store = await _storeService.GetByIdAsync(request.StoreId);
            request.Store = store;
            context.UserContext["store"] = store;
            context.UserContext["catalog"] = store.Catalog;
        }

        context.CopyArgumentsToUserContext();

        var currencies = await _currencyService.GetAllCurrenciesAsync();
        context.SetCurrencies(currencies, request.CultureName);
    }
}
