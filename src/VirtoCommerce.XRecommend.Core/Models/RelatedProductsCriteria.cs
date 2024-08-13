namespace VirtoCommerce.XRecommend.Core.Models;

public class RelatedProductsCriteria
{
    public string StoreId { get; set; }

    public string FallbackFilter { get; set; }

    public string ProductId { get; set; }

    public int MaxRecommendations { get; set; }
}
