using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using VirtoCommerce.Xapi.Core.BaseQueries;
using VirtoCommerce.Xapi.Core.Extensions;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Queries
{
    public class SearchHistoryQuery : Query<SearchHistoryResult>, ISearchQuery
    {
        public string UserId { get; set; }
        public string OrganizationId { get; set; }
        public string StoreId { get; set; }
        public int MaxCount { get; set; }

        public override IEnumerable<QueryArgument> GetArguments()
        {
            yield return Argument<NonNullGraphType<StringGraphType>>(nameof(StoreId), description: "Store Id");
            yield return Argument<NonNullGraphType<IntGraphType>>(nameof(MaxCount), description: "Max number of returned search queries");
        }

        public override void Map(IResolveFieldContext context)
        {
            UserId = context.GetCurrentUserId();
            OrganizationId = context.GetCurrentOrganizationId();
            StoreId = context.GetArgument<string>(nameof(StoreId));
            MaxCount = context.GetArgument<int>(nameof(MaxCount));

            if (MaxCount == 0)
            {
                MaxCount = ModuleConstants.DefaultMaxProducts;
            }
        }
    }
}
