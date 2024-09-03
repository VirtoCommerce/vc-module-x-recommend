using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Events;

public class UserEventChangedEvent : GenericChangedEntryEvent<UserEvent>
{
    public UserEventChangedEvent(IEnumerable<GenericChangedEntry<UserEvent>> changedEntries) : base(changedEntries)
    {
    }
}
