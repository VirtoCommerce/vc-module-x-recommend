using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.XRecommend.Core.Models;

public class HistoricalEventSearchCriteria : SearchCriteriaBase
{
    public string ProductId { get; set; }
    public string UserId { get; set; }
    public string StoreId { get; set; }
    public string SessionId { get; set; }
    public string EventType { get; set; }
}
