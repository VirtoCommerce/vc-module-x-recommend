namespace VirtoCommerce.XRecommend.Core.Models;

public interface ISearchQuery
{
    string UserId { get; set; }
    string OrganizationId { get; set; }
    string StoreId { get; set; }
}
