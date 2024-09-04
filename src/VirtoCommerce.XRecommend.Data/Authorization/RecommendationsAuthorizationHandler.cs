using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace VirtoCommerce.XRecommend.Data.Authorization;

public class RecommendationsAuthorizationRequirement : IAuthorizationRequirement
{
}

public class RecommendationsAuthorizationHandler : AuthorizationHandler<RecommendationsAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RecommendationsAuthorizationRequirement requirement)
    {
        var result = context.User.Identity.IsAuthenticated;

        if (result)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}
