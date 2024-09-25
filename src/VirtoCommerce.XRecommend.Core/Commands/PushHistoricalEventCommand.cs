using System.Collections.Generic;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.XRecommend.Core.Commands;

public class PushHistoricalEventCommand : ICommand<bool>
{
    public string StoreId { get; set; }

    public string UserId { get; set; }

    public string ProductId { get; set; }

    public List<string> ProductIds { get; set; } = [];

    public string SessionId { get; set; }

    public string EventType { get; set; }
}
