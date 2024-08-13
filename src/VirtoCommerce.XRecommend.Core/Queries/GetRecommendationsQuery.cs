using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using VirtoCommerce.XCatalog.Core.Queries;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Queries
{
    public class GetRecommendationsQuery : CatalogQueryBase<GetRecommendationsResult>
    {
        public string ProductId { get; set; }
        public string Model { get; set; }
        public string FallbackProductsQuery { get; set; }
        public int MaxRecommendations { get; set; }

        public override IEnumerable<QueryArgument> GetArguments()
        {
            foreach (var argument in base.GetArguments())
            {
                yield return argument;
            }

            yield return Argument<StringGraphType>(nameof(ProductId), description: "Product ID");
            yield return Argument<StringGraphType>(nameof(Model), description: "Recommendation model");
            yield return Argument<StringGraphType>(nameof(FallbackProductsQuery), description: "Fallback query");
            yield return Argument<StringGraphType>(nameof(MaxRecommendations), description: "Max number of returned recommendations");
        }

        public override void Map(IResolveFieldContext context)
        {
            base.Map(context);

            ProductId = context.GetArgument<string>(nameof(ProductId));
            Model = context.GetArgument<string>(nameof(Model));
            FallbackProductsQuery = context.GetArgument<string>(nameof(FallbackProductsQuery));
            MaxRecommendations = context.GetArgument<int>(nameof(MaxRecommendations));

            if (MaxRecommendations == 0)
            {
                MaxRecommendations = 5;
            }
        }
    }
}
