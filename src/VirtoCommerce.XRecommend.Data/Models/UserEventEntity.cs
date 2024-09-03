using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Data.Models;

public class UserEventEntity : AuditableEntity, IDataEntity<UserEventEntity, UserEvent>
{
    [StringLength(128)]
    public string ProductId { get; set; }

    [StringLength(128)]
    public string UserId { get; set; }

    [StringLength(128)]
    public string EventType { get; set; }

    public UserEvent ToModel(UserEvent model)
    {
        model.Id = Id;
        model.CreatedBy = CreatedBy;
        model.CreatedDate = CreatedDate;
        model.ModifiedBy = ModifiedBy;
        model.ModifiedDate = ModifiedDate;

        model.ProductId = ProductId;
        model.UserId = UserId;
        model.EventType = EventType;

        return model;
    }

    public UserEventEntity FromModel(UserEvent model, PrimaryKeyResolvingMap pkMap)
    {
        pkMap.AddPair(model, this);

        Id = model.Id;
        CreatedBy = model.CreatedBy;
        CreatedDate = model.CreatedDate;
        ModifiedBy = model.ModifiedBy;
        ModifiedDate = model.ModifiedDate;

        ProductId = model.ProductId;
        UserId = model.UserId;
        EventType = model.EventType;

        return this;
    }

    public void Patch(UserEventEntity target)
    {
        target.ProductId = ProductId;
        target.UserId = UserId;
        target.EventType = EventType;
    }
}
