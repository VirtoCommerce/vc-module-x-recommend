using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Events;

public class EventChangedEvent : GenericChangedEntryEvent<UserEvent>
{
    public EventChangedEvent(IEnumerable<GenericChangedEntry<UserEvent>> changedEntries) : base(changedEntries)
    {
    }
}
