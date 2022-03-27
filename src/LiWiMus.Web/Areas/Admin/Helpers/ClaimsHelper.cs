using System.Security.Claims;
using LiWiMus.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Web.Areas.Admin.Helpers;

public static class ClaimsHelper
{
    public static async Task AddPermissionClaim(this RoleManager<Role> roleManager, Role role, string permission)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
        {
            await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
        }
    }
}