using LiWiMus.Core.Permissions;
using LiWiMus.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace LiWiMus.Web.Shared.Configuration;

public static class ConfigurePolicies
{
    public static void AddPermissionPolicies(this AuthorizationOptions options)
    {
        var permissionNames = DefaultPermissions.GetAll().Select(p => p.Name);

        foreach (var permissionName in permissionNames)
        {
            options.AddPolicy(permissionName,
                policy => { policy.AddRequirements(new PermissionRequirement(permissionName)); });
        }
    }
}