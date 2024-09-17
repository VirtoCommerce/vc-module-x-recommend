using Microsoft.Data.SqlClient;
using VirtoCommerce.Platform.Data.Extensions;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Data.Repositories;

namespace VirtoCommerce.XRecommend.Data.SqlServer;

public class SqlServerRecommendRawDatabaseCommand : IRecommendRawDatabaseCommand
{
    public async Task<IList<string>> GetBoughtTogetherProductIdsAsync(XRecommendDbContext context, GetRecommendationsCriteria criteria)
    {
        var commandTemplate = @"
            WITH SessionIDs AS
            (
	            SELECT SessionId FROM HistoricalEvents
	            WHERE
	            StoreId = @storeId AND
	            ProductId = @productId AND
	            UserId <> @userId AND
	            EventType IN ('addToCart','placeOrder')
            )
            SELECT TOP (@limit) ProductId FROM HistoricalEvents he
            INNER JOIN SessionIDs s ON s.SessionId = he.SessionId
            WHERE he.ProductId <> @productId
            GROUP BY ProductId
            ORDER BY COUNT(ProductId) DESC";

        var parameters = new object[]
        {
            new SqlParameter("@storeId", criteria.StoreId),
            new SqlParameter("@productId", criteria.ProductId),
            new SqlParameter("@userId", criteria.UserId),
            new SqlParameter("@limit", criteria.MaxRecommendations),
        };

        var result = await context.ExecuteArrayAsync<string>(commandTemplate, parameters);
        return result;
    }
}
