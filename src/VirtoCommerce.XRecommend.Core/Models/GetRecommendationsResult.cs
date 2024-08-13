using System.Collections.Generic;
using VirtoCommerce.XCatalog.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Models;

public class GetRecommendationsResult
{
    public IList<ExpProduct> Products { get; set; } = new List<ExpProduct>();
}
