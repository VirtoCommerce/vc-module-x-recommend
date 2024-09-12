using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Services;

public interface IHistoricalEventSearchService : ISearchService<HistoricalEventSearchCriteria, HistoricalEventSearchResult, HistoricalEvent>
{
}
