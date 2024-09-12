using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.StoreModule.Core.Services;
using VirtoCommerce.Xapi.Core.Infrastructure;
using VirtoCommerce.XCatalog.Core.Queries;
using VirtoCommerce.XRecommend.Core;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Queries;
using VirtoCommerce.XRecommend.Core.Services;

namespace VirtoCommerce.XRecommend.Data.Queries;

public class GetRecentlyBrowsedQueryHandler : IQueryHandler<GetRecentlyBrowsedQuery, GetRecentlyBrowsedResult>
{
    private readonly IStoreService _storeService;
    private readonly IHistoricalEventSearchService _eventSearchService;
    private readonly IMediator _mediator;

    public GetRecentlyBrowsedQueryHandler(
        IStoreService storeService,
        IHistoricalEventSearchService eventSearchService,
        IMediator mediator)
    {
        _storeService = storeService;
        _eventSearchService = eventSearchService;
        _mediator = mediator;
    }

    public async Task<GetRecentlyBrowsedResult> Handle(GetRecentlyBrowsedQuery request, CancellationToken cancellationToken)
    {
        var result = new GetRecentlyBrowsedResult();

        var store = await _storeService.GetNoCloneAsync(request.StoreId);
        if (store == null || !store.Settings.GetValue<bool>(ModuleConstants.Settings.General.RecommendationsEnabled))
        {
            return result;
        }

        var searchCriteria = AbstractTypeFactory<HistoricalEventSearchCriteria>.TryCreateInstance();
        searchCriteria.UserId = request.UserId;
        searchCriteria.StoreId = request.StoreId;
        searchCriteria.EventType = ModuleConstants.EventTypes.Click;
        searchCriteria.Take = request.MaxProducts;

        var searchResult = await _eventSearchService.SearchAsync(searchCriteria);

        var productIds = searchResult.Results.Select(x => x.ProductId).ToList();

        var loadProductsQuery = GetLoadProductsQuery(request, productIds);
        var loadProductResponse = await _mediator.Send(loadProductsQuery, cancellationToken);

        result.Products = loadProductResponse.Products.OrderBy(x => productIds.IndexOf(x.Id)).ToList();

        return result;
    }

    private static LoadProductsQuery GetLoadProductsQuery(GetRecentlyBrowsedQuery request, IList<string> productIds)
    {
        return new LoadProductsQuery
        {
            StoreId = request.StoreId,
            ObjectIds = productIds,
            CultureName = request.CultureName,
            CurrencyCode = request.CurrencyCode,
            UserId = request.UserId,
            OrganizationId = request.OrganizationId,
            IncludeFields = request.IncludeFields,
        };
    }
}
