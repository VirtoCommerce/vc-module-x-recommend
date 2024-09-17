using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Data.Repositories;

namespace VirtoCommerce.XRecommend.Data.MySql;

public class MySqlRecommendRawDatabaseCommand : IRecommendRawDatabaseCommand
{
    public Task<IList<string>> GetBoughtTogetherProductIdsAsync(XRecommendDbContext context, GetRecommendationsCriteria criteria)
    {
        throw new NotImplementedException();
    }
}
