using GraphQL.Types;
using VirtoCommerce.XRecommend.Core.Commands;

namespace VirtoCommerce.XRecommend.Core.Schemas;

public class InputSaveSearchQueryType : InputObjectGraphType
{
    public InputSaveSearchQueryType()
    {
        Field<NonNullGraphType<StringGraphType>>(nameof(SaveSearchQueryCommand.StoreId));
        Field<NonNullGraphType<StringGraphType>>(nameof(SaveSearchQueryCommand.Query));
    }
}
