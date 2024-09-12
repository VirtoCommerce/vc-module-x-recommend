using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using VirtoCommerce.XCatalog.Core.Queries;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Queries
{
    public class GetRecentlyBrowsedQuery : CatalogQueryBase<GetRecentlyBrowsedResult>
    {
        public int MaxProducts { get; set; }

        public override IEnumerable<QueryArgument> GetArguments()
        {
            yield return Argument<NonNullGraphType<StringGraphType>>(nameof(StoreId), description: "Store Id");
            yield return Argument<StringGraphType>(nameof(CultureName), description: "Currency code (\"USD\")");
            yield return Argument<StringGraphType>(nameof(CurrencyCode), description: "Culture name (\"en-US\")");
            yield return Argument<IntGraphType>(nameof(MaxProducts), description: "Max number of returned browsed products");
        }

        public override void Map(IResolveFieldContext context)
        {
            base.Map(context);

            MaxProducts = context.GetArgument<int>(nameof(MaxProducts));

            if (MaxProducts == 0)
            {
                MaxProducts = ModuleConstants.DefaultMaxProducts;
            }
        }
    }
}
