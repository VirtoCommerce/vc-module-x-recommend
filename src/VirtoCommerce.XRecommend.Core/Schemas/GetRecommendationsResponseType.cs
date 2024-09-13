using GraphQL.Types;
using VirtoCommerce.Xapi.Core.Schemas;
using VirtoCommerce.XCatalog.Core.Schemas;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Schemas;

public class GetRecommendationsResponseType : ExtendableGraphType<GetRecommendationsResult>
{
    public GetRecommendationsResponseType()
    {
        ExtendableField<ListGraphType<ProductType>>("products", resolve: context => context.Source.Products);
    }
}
