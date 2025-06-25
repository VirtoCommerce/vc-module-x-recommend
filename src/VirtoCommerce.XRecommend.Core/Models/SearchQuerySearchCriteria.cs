using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.XRecommend.Core.Models;

public class SearchQuerySearchCriteria : SearchCriteriaBase, ISearchQuery
{
    public string UserId { get; set; }
    public string OrganizationId { get; set; }
    public string StoreId { get; set; }
}
