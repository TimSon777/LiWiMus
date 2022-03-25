using System.Security.Claims;
using LiWiMus.Core.Constants;
using LiWiMus.Core.Entities;
using LiWiMus.Core.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LiWiMus.Infrastructure.Data;

public static class ApplicationContextSeed
{
    public static async Task SeedAsync(ApplicationContext applicationContext,
                                       ILogger logger,
                                       UserManager<User> userManager, RoleManager<Role> roleManager,
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
            await SeedAdminAsync(userManager, roleManager, adminSettings);
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

    private static async Task SeedRolesAsync(this RoleManager<Role> roleManager)
    {
        foreach (var role in Roles.GetPreconfiguredRoles())
        {
            if (!await roleManager.RoleExistsAsync(role.Name))
            {
                await roleManager.CreateAsync(role);
            }
        }
    }

    private static async Task SeedAdminAsync(UserManager<User> userManager, RoleManager<Role> roleManager, AdminSettings adminSettings)
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

        await userManager.AddToRoleAsync(admin, Roles.Admin.Name);
        await userManager.AddToRoleAsync(admin, Roles.Moderator.Name);
        await userManager.AddToRoleAsync(admin, Roles.User.Name);

        var adminRole = await roleManager.FindByNameAsync(Roles.Admin.Name);
        await roleManager.AddPermissionClaim(adminRole, Permissions.GetAllPermissions());
    }

    private static async Task AddPermissionClaim(this RoleManager<Role> roleManager, Role role, List<string> permissions)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        foreach (var permission in permissions)
        {
            if (!allClaims.Any(claim => claim.Type == Permissions.ClaimType && claim.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim(Permissions.ClaimType, permission));
            }
        }
    }
}