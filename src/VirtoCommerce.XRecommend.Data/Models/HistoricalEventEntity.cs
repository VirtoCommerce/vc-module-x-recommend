using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Data.Models;

public class HistoricalEventEntity : AuditableEntity, IDataEntity<HistoricalEventEntity, HistoricalEvent>
{
    [StringLength(128)]
    public string ProductId { get; set; }

    [StringLength(128)]
    public string UserId { get; set; }

    [StringLength(128)]
    public string StoreId { get; set; }

    [StringLength(128)]
    public string EventType { get; set; }

    public HistoricalEvent ToModel(HistoricalEvent model)
    {
        model.Id = Id;
        model.CreatedBy = CreatedBy;
        model.CreatedDate = CreatedDate;
        model.ModifiedBy = ModifiedBy;
        model.ModifiedDate = ModifiedDate;

        model.ProductId = ProductId;
        model.UserId = UserId;
        model.StoreId = StoreId;
        model.EventType = EventType;

        return model;
    }

    public HistoricalEventEntity FromModel(HistoricalEvent model, PrimaryKeyResolvingMap pkMap)
    {
        pkMap.AddPair(model, this);

        Id = model.Id;
        CreatedBy = model.CreatedBy;
        CreatedDate = model.CreatedDate;
        ModifiedBy = model.ModifiedBy;
        ModifiedDate = model.ModifiedDate;

        ProductId = model.ProductId;
        UserId = model.UserId;
        StoreId = model.StoreId;
        EventType = model.EventType;

        return this;
    }

    public void Patch(HistoricalEventEntity target)
    {
        target.ProductId = ProductId;
        target.UserId = UserId;
        target.StoreId = StoreId;
        target.EventType = EventType;
    }
}
