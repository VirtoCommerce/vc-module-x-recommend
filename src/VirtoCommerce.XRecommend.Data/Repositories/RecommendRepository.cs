using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.XRecommend.Data.Models;

namespace VirtoCommerce.XRecommend.Data.Repositories
{
    public class RecommendRepository : DbContextRepositoryBase<XRecommendDbContext>, IRecommendRepository
    {
        public RecommendRepository(XRecommendDbContext dbContext, IUnitOfWork unitOfWork = null) : base(dbContext, unitOfWork)
        {
        }

        public IQueryable<EventEntity> Events => DbContext.Set<EventEntity>();

        public virtual async Task<IList<EventEntity>> GetEventsByIdsAsync(IList<string> ids, string responseGroup)
        {
            return await Events.Where(x => ids.Contains(x.Id)).ToListAsync();
        }
    }
}
