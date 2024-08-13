using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.XCatalog.Core.Models;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Services;

namespace VirtoCommerce.XRecommend.Data.Services;

public class RecommendService : IRecommendService
{
    public Task<IList<ExpProduct>> GetRelatedProductsAsync(RelatedProductsCriteria relatedProductsCriteria)
    {
        return null;
    }
}
