using GraphQL.Types;

namespace VirtoCommerce.XRecommend.Core.Schemas;

public class InputPushHistoricalEventType : InputObjectGraphType
{
    public InputPushHistoricalEventType()
    {
        Field<StringGraphType>("storeId");
        Field<StringGraphType>("productId");
        Field<ListGraphType<StringGraphType>>("productIds");
        Field<StringGraphType>("sessionId");
        Field<StringGraphType>("eventType");
    }
}
