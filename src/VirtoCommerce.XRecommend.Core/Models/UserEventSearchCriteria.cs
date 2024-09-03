using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.XRecommend.Core.Models;

public class UserEventSearchCriteria : SearchCriteriaBase
{
    public string ProductId { get; set; }
    public string UserId { get; set; }
}
