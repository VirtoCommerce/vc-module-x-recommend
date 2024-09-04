using GraphQL.Types;
using VirtoCommerce.XCatalog.Core.Schemas;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Schemas;

public class GetRecentlyBrowsedResponseType : ObjectGraphType<GetRecentlyBrowsedResult>
{
    public GetRecentlyBrowsedResponseType()
    {
        Field<ListGraphType<ProductType>>("products", resolve: context => context.Source.Products);
    }
}
