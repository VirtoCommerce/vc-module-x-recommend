using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Services;

public interface ISearchQuerySearchService : ISearchService<SearchQuerySearchCriteria, SearchQuerySearchResult, SearchQuery>;
