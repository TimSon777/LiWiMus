using System.Reflection;
using System.Security.Claims;
using LiWiMus.Core.Entities;
using LiWiMus.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Web.Areas.Admin.Helpers;

public static class ClaimsHelper
{
    public static void GetPermissions(this List<RoleClaimsViewModel> allPermissions, Type policy, string roleId)
    {
        FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);

        foreach (FieldInfo fi in fields)
        {
            allPermissions.Add(new RoleClaimsViewModel { Value = fi.GetValue(null).ToString(), Type = "Permissions" });
        }
    }

    public static async Task AddPermissionClaim(this RoleManager<Role> roleManager, Role role, string permission)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
        {
            await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
        }
    }
}