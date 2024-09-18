using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Data.Models;

namespace VirtoCommerce.XRecommend.Data.Repositories
{
    public interface IRecommendRepository : IRepository
    {
        public IQueryable<HistoricalEventEntity> HistoricalEvents { get; }

        public Task<IList<HistoricalEventEntity>> GetHistoricalEventsByIdsAsync(IList<string> ids, string responseGroup);

        Task<IList<string>> GetBoughtTogetherProductIdsAsync(GetRecommendationsCriteria criteria, int minConversionEventsCount);
    }
}
