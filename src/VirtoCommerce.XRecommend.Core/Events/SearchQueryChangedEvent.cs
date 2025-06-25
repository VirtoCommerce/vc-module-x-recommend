using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Events;

public class SearchQueryChangedEvent(IEnumerable<GenericChangedEntry<SearchQuery>> changedEntries)
    : GenericChangedEntryEvent<SearchQuery>(changedEntries);
