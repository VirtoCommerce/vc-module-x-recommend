using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Data.Models;

namespace VirtoCommerce.XRecommend.Data.Repositories
{
    public class RecommendRepository : DbContextRepositoryBase<XRecommendDbContext>, IRecommendRepository
    {
        readonly IRecommendRawDatabaseCommand _rawDatabaseCommand;

        public RecommendRepository(
            IRecommendRawDatabaseCommand rawDatabaseCommand,
            XRecommendDbContext dbContext,
            IUnitOfWork unitOfWork = null)
            : base(dbContext, unitOfWork)
        {
            _rawDatabaseCommand = rawDatabaseCommand;

        }

        public IQueryable<HistoricalEventEntity> HistoricalEvents => DbContext.Set<HistoricalEventEntity>();

        public virtual async Task<IList<HistoricalEventEntity>> GetHistoricalEventsByIdsAsync(IList<string> ids, string responseGroup)
        {
            return await HistoricalEvents.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public virtual Task<IList<string>> GetBoughtTogetherProductIdsAsync(GetRecommendationsCriteria criteria)
        {
            return _rawDatabaseCommand.GetBoughtTogetherProductIdsAsync(DbContext, criteria);
        }
    }
}
