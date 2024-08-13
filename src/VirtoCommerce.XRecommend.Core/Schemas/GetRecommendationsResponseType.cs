using GraphQL.Types;
using VirtoCommerce.XCatalog.Core.Schemas;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Schemas;

public class GetRecommendationsResponseType : ObjectGraphType<GetRecommendationsResult>
{
    public GetRecommendationsResponseType()
    {
        Field<ListGraphType<ProductType>>("products", resolve: context => context.Source.Products);
    }
}
