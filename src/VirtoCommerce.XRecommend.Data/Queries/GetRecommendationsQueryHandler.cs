using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Xapi.Core.Infrastructure;
using VirtoCommerce.XCatalog.Core.Queries;
using VirtoCommerce.XDigitalCatalog.Queries;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Queries;
using VirtoCommerce.XRecommend.Core.Services;

namespace VirtoCommerce.XRecommend.Data.Queries;

public class GetRecommendationsQueryHandler : IQueryHandler<GetRecommendationsQuery, GetRecommendationsResult>
{
    private readonly IRecommendationsService _recommendService;
    private readonly IMediator _mediator;

    public GetRecommendationsQueryHandler(IRecommendationsService recommendService, IMediator mediator)
    {
        _recommendService = recommendService;
        _mediator = mediator;
    }

    public virtual async Task<GetRecommendationsResult> Handle(GetRecommendationsQuery request, CancellationToken cancellationToken)
    {
        var recommendationsCriteria = GetRelatedProductsCriteria(request);
        var recommendedProductIds = await _recommendService.GetRecommendationsAsync(recommendationsCriteria);

        var loadProductsQuery = GetLoadProductsQuery(request, recommendedProductIds);
        var recommendedProducts = await _mediator.Send(loadProductsQuery, cancellationToken);

        var result = new GetRecommendationsResult
        {
            Products = recommendedProducts.Products.OrderBy(x => recommendedProductIds.IndexOf(x.Id)).ToList(),
        };

        // add fallback products if needed
        if (!string.IsNullOrEmpty(request.FallbackProductsFilter) && request.MaxRecommendations > result.Products.Count)
        {
            var searchProductsQuery = GetSearchProductQuery(request, request.MaxRecommendations - result.Products.Count);
            var fallbackProductsResult = await _mediator.Send(searchProductsQuery, cancellationToken);
            result.Products.AddRange(fallbackProductsResult.Results);
        }

        return result;
    }

    private static GetRecommendationsCriteria GetRelatedProductsCriteria(GetRecommendationsQuery request)
    {
        return new GetRecommendationsCriteria
        {
            StoreId = request.StoreId,
            MaxRecommendations = request.MaxRecommendations,
            ProductId = request.ProductId,
        };
    }

    private static LoadProductsQuery GetLoadProductsQuery(GetRecommendationsQuery request, IList<string> recommendedProductIds)
    {
        return new LoadProductsQuery
        {
            StoreId = request.StoreId,
            ObjectIds = recommendedProductIds,
            CultureName = request.CultureName,
            CurrencyCode = request.CurrencyCode,
            UserId = request.UserId,
            OrganizationId = request.OrganizationId,
            IncludeFields = request.IncludeFields,
        };
    }

    private static SearchProductQuery GetSearchProductQuery(GetRecommendationsQuery request, int take)
    {
        return new SearchProductQuery
        {
            StoreId = request.StoreId,
            CultureName = request.CultureName,
            CurrencyCode = request.CurrencyCode,
            UserId = request.UserId,
            OrganizationId = request.OrganizationId,
            IncludeFields = request.IncludeFields,
            Take = take,
            Filter = request.FallbackProductsFilter,
        };
    }
}
