using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VirtoCommerce.SearchModule.Core.Services;
using VirtoCommerce.StoreModule.Core.Services;
using VirtoCommerce.Xapi.Core.Infrastructure;
using VirtoCommerce.XDigitalCatalog.Queries;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Queries;
using VirtoCommerce.XRecommend.Core.Services;

namespace VirtoCommerce.XRecommend.Data.Queries;

public class GetRecommendationsQueryHandler : IQueryHandler<GetRecommendationsQuery, GetRecommendationsResult>
{
    private readonly IRecommendService _recommendService;
    private readonly IStoreService _storeService;
    private readonly ISearchProvider _searchProvider;
    private readonly IMediator _mediator;

    public GetRecommendationsQueryHandler(IRecommendService recommendService, IStoreService storeService, ISearchProvider searchProvider, IMediator mediator)
    {
        _recommendService = recommendService;
        _storeService = storeService;
        _searchProvider = searchProvider;
        _mediator = mediator;
    }

    public virtual async Task<GetRecommendationsResult> Handle(GetRecommendationsQuery request, CancellationToken cancellationToken)
    {
        var searchProductsQuery = new SearchProductQuery
        {
            StoreId = request.StoreId,
            UserId = request.UserId,
            OrganizationId = request.OrganizationId,
            Take = request.MaxRecommendations,
            Skip = 0,
            IncludeFields = request.IncludeFields,
            CurrencyCode = request.CurrencyCode,
            CultureName = request.CultureName,
        };

        var searchProductsResult = await _mediator.Send(searchProductsQuery);

        var result = new GetRecommendationsResult()
        {
            Products = searchProductsResult.Results,
        };

        return result;
    }
}
