using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Events;

public class HistoricalEventChangingEvent : GenericChangedEntryEvent<HistoricalEvent>
{
    public HistoricalEventChangingEvent(IEnumerable<GenericChangedEntry<HistoricalEvent>> changedEntries) : base(changedEntries)
    {
    }
}