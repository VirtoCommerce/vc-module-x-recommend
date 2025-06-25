using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.XRecommend.Core.Commands;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Services;

namespace VirtoCommerce.XRecommend.Data.Commands;

public class SaveSearchQueryCommandHandler(ISearchQueryService searchQueryService)
    : IRequestHandler<SaveSearchQueryCommand, bool>
{
    public async Task<bool> Handle(SaveSearchQueryCommand request, CancellationToken cancellationToken)
    {
        if (request.Query.IsNullOrEmpty())
        {
            return false;
        }

        var query = AbstractTypeFactory<SearchQuery>.TryCreateInstance();
        query.UserId = request.UserId;
        query.OrganizationId = request.OrganizationId;
        query.StoreId = request.StoreId;
        query.Query = request.Query;

        await searchQueryService.SaveChangesAsync([query]);

        return true;
    }
}
