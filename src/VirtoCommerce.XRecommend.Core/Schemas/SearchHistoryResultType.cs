using GraphQL.Types;
using VirtoCommerce.Xapi.Core.Schemas;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Schemas;

public class SearchHistoryResultType : ExtendableGraphType<SearchHistoryResult>
{
    public SearchHistoryResultType()
    {
        ExtendableField<ListGraphType<StringGraphType>>("Queries", resolve: context => context.Source.Queries);
    }
}
