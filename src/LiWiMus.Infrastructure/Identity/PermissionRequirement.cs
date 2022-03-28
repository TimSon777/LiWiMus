using Microsoft.AspNetCore.Authorization;

namespace LiWiMus.Infrastructure.Identity;

internal class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}