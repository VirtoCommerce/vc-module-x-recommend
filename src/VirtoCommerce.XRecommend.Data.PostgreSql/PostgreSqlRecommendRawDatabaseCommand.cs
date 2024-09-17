using Npgsql;
using VirtoCommerce.Platform.Data.Extensions;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Data.Repositories;

namespace VirtoCommerce.XRecommend.Data.PostgreSql;

public class PostgreSqlRecommendRawDatabaseCommand : IRecommendRawDatabaseCommand
{
    public async Task<IList<string>> GetBoughtTogetherProductIdsAsync(XRecommendDbContext context, GetRecommendationsCriteria criteria)
    {
        var commandTemplate = @"
            WITH SessionIDs AS
            (
	            SELECT ""SessionId"" FROM ""HistoricalEvents""
	            WHERE
	            ""StoreId"" = @storeId AND
	            ""ProductId"" = @productId AND
	            ""UserId"" <> @userId AND
	            ""EventType"" IN ('addToCart','placeOrder')
            )
            SELECT ""ProductId"" FROM ""HistoricalEvents"" he
            INNER JOIN SessionIDs s ON s.""SessionId"" = he.""SessionId""
            WHERE he.""ProductId"" <> @productId
            GROUP BY ""ProductId""
            ORDER BY COUNT(""ProductId"") DESC
            LIMIT @limit";

        var parameters = new object[]
        {
            new NpgsqlParameter("@storeId", criteria.StoreId),
            new NpgsqlParameter("@productId", criteria.ProductId),
            new NpgsqlParameter("@userId", criteria.UserId),
            new NpgsqlParameter("@limit", criteria.MaxRecommendations),
        };

        var result = await context.ExecuteArrayAsync<string>(commandTemplate, parameters);
        return result;
    }
}
