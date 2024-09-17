using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Data.Repositories;

public interface IRecommendRawDatabaseCommand
{
    Task<IList<string>> GetBoughtTogetherProductIdsAsync(XRecommendDbContext context, GetRecommendationsCriteria criteria);
}
