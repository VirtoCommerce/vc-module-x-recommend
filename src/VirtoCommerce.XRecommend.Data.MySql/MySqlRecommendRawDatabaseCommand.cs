using MySqlConnector;
using VirtoCommerce.Platform.Data.Extensions;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Data.Repositories;

namespace VirtoCommerce.XRecommend.Data.MySql;

public class MySqlRecommendRawDatabaseCommand : IRecommendRawDatabaseCommand
{
    public async Task<IList<string>> GetBoughtTogetherProductIdsAsync(XRecommendDbContext context, GetRecommendationsCriteria criteria)
    {
        var commandTemplate = @"
            SELECT ProductId 
            FROM HistoricalEvents he
            INNER JOIN (
                SELECT SessionId 
                FROM HistoricalEvents
                WHERE StoreId = @storeId 
                  AND ProductId = @productId 
                  AND UserId <> @userId 
                  AND EventType IN ('addToCart', 'placeOrder')
            ) s ON s.SessionId = he.SessionId
            WHERE he.ProductId <> @productId
            GROUP BY ProductId
            ORDER BY COUNT(ProductId) DESC
            LIMIT @limit;";

        var parameters = new object[]
        {
            new MySqlParameter("@storeId", criteria.StoreId),
            new MySqlParameter("@productId", criteria.ProductId),
            new MySqlParameter("@userId", criteria.UserId),
            new MySqlParameter("@limit", criteria.MaxRecommendations),
        };

        var result = await context.ExecuteArrayAsync<string>(commandTemplate, parameters);
        return result;
    }
}
