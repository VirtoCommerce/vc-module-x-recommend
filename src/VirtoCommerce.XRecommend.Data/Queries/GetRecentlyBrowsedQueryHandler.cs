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
using static VirtoCommerce.XRecommend.Core.ModuleConstants;

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

        var browsedProductsResult = await _eventSearchService.SearchAsync(new HistoricalEventSearchCriteria
        {
            UserId = request.UserId,
            StoreId = request.StoreId,
            EventType = EventTypes.Click,
            Take = request.MaxProducts,
        });

        var browsedProductsIds = browsedProductsResult.Results.Select(x => x.ProductId).ToList();

        var loadProductsQuery = GetLoadProductsQuery(request, browsedProductsIds);
        var browsedProducts = await _mediator.Send(loadProductsQuery, cancellationToken);

        result = new GetRecentlyBrowsedResult
        {
            Products = browsedProducts.Products.OrderBy(x => browsedProductsIds.IndexOf(x.Id)).ToList(),
        };

        return result;
    }

    private static LoadProductsQuery GetLoadProductsQuery(GetRecentlyBrowsedQuery request, IList<string> browsedProductsIds)
    {
        return new LoadProductsQuery
        {
            StoreId = request.StoreId,
            ObjectIds = browsedProductsIds,
            CultureName = request.CultureName,
            CurrencyCode = request.CurrencyCode,
            UserId = request.UserId,
            OrganizationId = request.OrganizationId,
            IncludeFields = request.IncludeFields,
        };
    }
}
