using LiWiMus.Core.Constants;
using LiWiMus.Core.Entities;
using LiWiMus.Web.Areas.Admin.Helpers;
using LiWiMus.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class PermissionController : Controller
{
    private readonly RoleManager<Role> _roleManager;

    public PermissionController(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }
    public async Task<ActionResult> Index(string roleId)
    {
        var model = new PermissionViewModel();
        var allPermissions = new List<RoleClaimsViewModel>();
        allPermissions.GetPermissions(typeof(Permissions.Artist), roleId);
        var role = await _roleManager.FindByIdAsync(roleId);
        model.RoleId = roleId;
        var claims = await _roleManager.GetClaimsAsync(role);
        var allClaimValues = allPermissions.Select(a => a.Value).ToList();
        var roleClaimValues = claims.Select(a => a.Value).ToList();
        var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
        foreach (var permission in allPermissions)
        {
            if (authorizedClaims.Any(a => a == permission.Value))
            {
                permission.Selected = true;
            }
        }
        model.RoleClaims = allPermissions;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(PermissionViewModel model)
    {
        var role = await _roleManager.FindByIdAsync(model.RoleId);
        var claims = await _roleManager.GetClaimsAsync(role);
        foreach (var claim in claims)
        {
            await _roleManager.RemoveClaimAsync(role, claim);
        }
        var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
        foreach (var claim in selectedClaims)
        {
            await _roleManager.AddPermissionClaim(role, claim.Value);
        }
        return RedirectToAction("Index", "Roles", new { area = "Admin" });
    }
}