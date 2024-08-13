using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.XCatalog.Core.Models;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Services;

public interface IRecommendService
{
    Task<IList<ExpProduct>> GetRelatedProductsAsync(RelatedProductsCriteria relatedProductsCriteria);
}
