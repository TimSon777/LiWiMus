using Microsoft.AspNetCore.Authorization;

namespace LiWiMus.Web.Permission;

internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    public PermissionAuthorizationHandler(){}
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var permissions = context.User.Claims.Where(x => x.Type == "Permission" &&
                                                          x.Value == requirement.Permission &&
                                                          x.Issuer == "LOCAL AUTHORITY");
        if (!permissions.Any())
        {
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}