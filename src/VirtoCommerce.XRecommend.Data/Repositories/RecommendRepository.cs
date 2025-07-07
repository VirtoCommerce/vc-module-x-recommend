using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Data.Models;

namespace VirtoCommerce.XRecommend.Data.Repositories
{
    public class RecommendRepository(XRecommendDbContext dbContext, IUnitOfWork unitOfWork = null)
        : DbContextRepositoryBase<XRecommendDbContext>(dbContext, unitOfWork), IRecommendRepository
    {
        public IQueryable<HistoricalEventEntity> HistoricalEvents => DbContext.Set<HistoricalEventEntity>();

        public IQueryable<SearchQueryEntity> SearchQueries => DbContext.Set<SearchQueryEntity>();

        public virtual async Task<IList<HistoricalEventEntity>> GetHistoricalEventsByIdsAsync(IList<string> ids, string responseGroup)
        {
            return await HistoricalEvents.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public virtual async Task<IList<string>> GetBoughtTogetherProductIdsAsync(GetRecommendationsCriteria criteria, int minConversionEventsCount)
        {
            if (minConversionEventsCount > 0)
            {
                var count = await HistoricalEvents.CountAsync(x => x.StoreId == criteria.StoreId
                                   && (x.EventType == "addToCart" || x.EventType == "placeOrder"));

                if (count < minConversionEventsCount)
                {
                    return new List<string>();
                }
            }

            var sessionIds = HistoricalEvents.Where(x => x.StoreId == criteria.StoreId
                                && x.ProductId == criteria.ProductId
                                && x.UserId != criteria.UserId
                                && (x.EventType == "addToCart" || x.EventType == "placeOrder"))
                            .Select(x => x.SessionId);

            var result = await HistoricalEvents.Join(sessionIds, x => x.SessionId, y => y, (x, y) => x)
                .Where(x => x.ProductId != criteria.ProductId)
                .GroupBy(x => x.ProductId)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .Take(criteria.MaxRecommendations)
                .ToListAsync();

            return result;
        }

        public virtual async Task<IList<SearchQueryEntity>> GetSearchQueriesByIdsAsync(IList<string> ids, string responseGroup)
        {
            if (ids.IsNullOrEmpty())
            {
                return [];
            }

            return ids.Count == 1
                ? await SearchQueries.Where(x => x.Id == ids.First()).ToListAsync()
                : await SearchQueries.Where(x => ids.Contains(x.Id)).ToListAsync();
        }
    }
}
