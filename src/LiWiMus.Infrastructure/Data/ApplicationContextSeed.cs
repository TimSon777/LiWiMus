using System.Security.Claims;
using LiWiMus.Core.Permissions;
using LiWiMus.Core.Roles;
using LiWiMus.Core.Settings;
using LiWiMus.Core.Users;
using LiWiMus.Infrastructure.Data.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LiWiMus.Infrastructure.Data;

public static class ApplicationContextSeed
{
    public static async Task SeedAsync(ApplicationContext applicationContext,
                                       ILogger logger,
                                       UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager,
                                       AdminSettings adminSettings,
                                       int retry = 0)
    {
        var retryForAvailability = retry;
        try
        {
            if (applicationContext.Database.IsMySql())
            {
                await applicationContext.Database.MigrateAsync();
            }

            await roleManager.SeedRolesAsync();
            await SeedAdminAsync(userManager, adminSettings);
            await applicationContext.SeedPlansAndPermissionsAsync();
        }
        catch (Exception ex)
        {
            if (retryForAvailability >= 10) throw;

            retryForAvailability++;

            logger.LogError("Error while seeding database: {Message}", ex.Message);
            await SeedAsync(applicationContext, logger, userManager, roleManager, adminSettings, retryForAvailability);
            throw;
        }
    }

    private static async Task SeedRolesAsync(this RoleManager<IdentityRole<int>> roleManager)
    {
        foreach (var (role, permissions) in DefaultRoles.GetRolesWithPermissions())
        {
            if (!await roleManager.RoleExistsAsync(role.Name))
            {
                await roleManager.CreateAsync(role);
                await roleManager.AddPermissionClaim(role, permissions);
            }
        }
    }

    private static async Task SeedAdminAsync(UserManager<User> userManager, AdminSettings adminSettings)
    {
        var admin = new User
        {
            UserName = adminSettings.UserName,
            Email = adminSettings.Email,
            EmailConfirmed = true
        };
        var user = await userManager.FindByEmailAsync(admin.Email);
        if (user == null)
        {
            await userManager.CreateAsync(admin, adminSettings.Password);
        }
        else
        {
            admin = user;
        }

        await userManager.AddToRoleAsync(admin, DefaultRoles.Admin.Name);
    }

    private static async Task AddPermissionClaim(this RoleManager<IdentityRole<int>> roleManager,
                                                 IdentityRole<int> role, List<string> permissions)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        foreach (var permission in permissions)
        {
            if (!allClaims.Any(claim => claim.Type == Permission.ClaimType && claim.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim(Permission.ClaimType, permission));
            }
        }
    }
}