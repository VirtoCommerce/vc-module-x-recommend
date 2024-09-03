using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.XRecommend.Data.Repositories
{
    public class RecommendRepository : DbContextRepositoryBase<RecommendDbContext>, IRecommendRepository
    {
        public RecommendRepository(RecommendDbContext dbContext, IUnitOfWork unitOfWork = null) : base(dbContext, unitOfWork)
        {
        }
    }
}
