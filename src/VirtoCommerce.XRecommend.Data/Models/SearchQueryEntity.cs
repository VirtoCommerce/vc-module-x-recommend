using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Data.Models;

public class SearchQueryEntity : AuditableEntity, IDataEntity<SearchQueryEntity, SearchQuery>
{
    [Required]
    [MaxLength(128)]
    public string UserId { get; set; }

    [MaxLength(128)]
    public string OrganizationId { get; set; }

    [Required]
    [MaxLength(128)]
    public string StoreId { get; set; }

    [Required]
    [MaxLength(2048)]
    public string Query { get; set; }

    public virtual SearchQuery ToModel(SearchQuery model)
    {
        model.Id = Id;
        model.CreatedBy = CreatedBy;
        model.CreatedDate = CreatedDate;
        model.ModifiedBy = ModifiedBy;
        model.ModifiedDate = ModifiedDate;

        model.UserId = UserId;
        model.OrganizationId = OrganizationId;
        model.StoreId = StoreId;
        model.Query = Query;

        return model;
    }

    public virtual SearchQueryEntity FromModel(SearchQuery model, PrimaryKeyResolvingMap pkMap)
    {
        pkMap.AddPair(model, this);

        Id = model.Id;
        CreatedBy = model.CreatedBy;
        CreatedDate = model.CreatedDate;
        ModifiedBy = model.ModifiedBy;
        ModifiedDate = model.ModifiedDate;

        UserId = model.UserId;
        OrganizationId = model.OrganizationId;
        StoreId = model.StoreId;
        Query = model.Query;

        return this;
    }

    public virtual void Patch(SearchQueryEntity target)
    {
        target.UserId = UserId;
        target.OrganizationId = OrganizationId;
        target.StoreId = StoreId;
        target.Query = Query;
    }
}
