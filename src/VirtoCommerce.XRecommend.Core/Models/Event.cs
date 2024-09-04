using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.XRecommend.Core.Models;

public class Event : AuditableEntity, ICloneable
{
    public string ProductId { get; set; }

    public string UserId { get; set; }

    public string StoreId { get; set; }

    public string EventType { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}
