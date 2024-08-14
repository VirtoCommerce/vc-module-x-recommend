using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Services;

public interface IRecommendationsService
{
    Task<IList<string>> GetRecommendationsAsync(GetRecommendationsCriteria criteria);
}
