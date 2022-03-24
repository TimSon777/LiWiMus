using Microsoft.AspNetCore.Authorization;

namespace LiWiMus.Web.Permission;

internal class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}