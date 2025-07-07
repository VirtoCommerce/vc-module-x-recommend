using VirtoCommerce.Xapi.Core.Infrastructure;
using ISearchQuery = VirtoCommerce.XRecommend.Core.Models.ISearchQuery;

namespace VirtoCommerce.XRecommend.Core.Commands;

public class SaveSearchQueryCommand : ICommand<bool>, ISearchQuery
{
    public string UserId { get; set; }
    public string OrganizationId { get; set; }
    public string StoreId { get; set; }
    public string Query { get; set; }
}
