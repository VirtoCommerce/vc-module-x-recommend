namespace VirtoCommerce.XRecommend.Core.Models;

public class GetRecommendationsCriteria
{
    public string StoreId { get; set; }

    public string UserId { get; set; }

    public string ProductId { get; set; }

    public int MaxRecommendations { get; set; }
}
