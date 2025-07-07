using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.XRecommend.Core.Models;

public class SearchQuery : AuditableEntity, ISearchQuery, ICloneable
{
    public string UserId { get; set; }
    public string OrganizationId { get; set; }
    public string StoreId { get; set; }
    public string Query { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}
