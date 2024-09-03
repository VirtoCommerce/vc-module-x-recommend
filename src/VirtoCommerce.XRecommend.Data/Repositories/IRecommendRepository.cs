using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.XRecommend.Data.Models;

namespace VirtoCommerce.XRecommend.Data.Repositories
{
    public interface IRecommendRepository : IRepository
    {
        public IQueryable<UserEventEntity> UserEvents { get; }

        public Task<IList<UserEventEntity>> GetUserEventsByIdsAsync(IList<string> ids, string responseGroup);
    }
}
