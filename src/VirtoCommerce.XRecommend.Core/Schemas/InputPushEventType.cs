using GraphQL.Types;

namespace VirtoCommerce.XRecommend.Core.Schemas;

public class InputPushEventType : InputObjectGraphType
{
    public InputPushEventType()
    {
        Field<StringGraphType>("storeId");
        Field<StringGraphType>("productId");
        Field<StringGraphType>("eventType");
    }
}
