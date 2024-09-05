using GraphQL.Types;

namespace VirtoCommerce.XRecommend.Core.Schemas;

public class InputPushHistoricalEventType : InputObjectGraphType
{
    public InputPushHistoricalEventType()
    {
        Field<StringGraphType>("storeId");
        Field<StringGraphType>("productId");
        Field<StringGraphType>("eventType");
    }
}
