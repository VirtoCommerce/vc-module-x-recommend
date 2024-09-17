using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Data.Repositories;

namespace VirtoCommerce.XRecommend.Data.PostgreSql;

public class PostgreSqlRecommendRawDatabaseCommand : IRecommendRawDatabaseCommand
{
    public Task<IList<string>> GetBoughtTogetherProductIdsAsync(XRecommendDbContext context, GetRecommendationsCriteria criteria)
    {
        throw new NotImplementedException();
    }
}
