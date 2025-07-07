using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.StoreModule.Core.Services;
using VirtoCommerce.XRecommend.Core.Commands;
using VirtoCommerce.XRecommend.Core.Queries;

namespace VirtoCommerce.XRecommend.Data.Authorization;

public class SearchHistoryAuthorizationRequirement : IAuthorizationRequirement;

public class SearchHistoryAuthorizationHandler(
    Func<UserManager<ApplicationUser>> userManagerFactory,
    IStoreService storeService)
    : AuthorizationHandler<SearchHistoryAuthorizationRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SearchHistoryAuthorizationRequirement requirement)
    {
        var authenticated = context.User.Identity?.IsAuthenticated ?? false;

        var authorized = authenticated && context.Resource switch
        {
            SearchHistoryQuery query => await CanAccessStore(query.StoreId, context),
            SaveSearchQueryCommand command => await CanAccessStore(command.StoreId, context),
            _ => false,
        };

        if (authorized)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }

    private async Task<bool> CanAccessStore(string storeId, AuthorizationHandlerContext context)
    {
        var userStoreId = await GetUserStoreId(context);

        if (userStoreId.EqualsIgnoreCase(storeId))
        {
            return true;
        }

        var store = await storeService.GetByIdAsync(storeId);

        return store?.TrustedGroups?.Any(userStoreId.EqualsIgnoreCase) ?? false;
    }

    private async Task<string> GetUserStoreId(AuthorizationHandlerContext context)
    {
        using var userManager = userManagerFactory();
        var userId = context.User.GetUserId();
        var user = await userManager.FindByIdAsync(userId);

        return user?.StoreId;
    }
}
