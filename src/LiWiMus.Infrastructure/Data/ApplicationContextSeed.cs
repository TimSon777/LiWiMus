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

            if (!await applicationContext.Roles.AnyAsync())
            {
                await applicationContext.Roles.AddRangeAsync(GetPreconfiguredRoles());

                await applicationContext.SaveChangesAsync();
            }

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

    private static IEnumerable<Role> GetPreconfiguredRoles()
    {
        return new List<Role>
        {
            Roles.Admin,
            Roles.Moderator,
            Roles.Artist,
            Roles.User
        };
    }

    private static async Task SeedAdminAsync(UserManager<User> userManager, RoleManager<Role> roleManager, AdminSettings adminSettings)
    {
        var defaultUser = new User
        {
            UserName = adminSettings.UserName,
            Email = adminSettings.Email,
            EmailConfirmed = true
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, adminSettings.Password);
                await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                await userManager.AddToRoleAsync(defaultUser, Roles.Moderator.ToString());
                await userManager.AddToRoleAsync(defaultUser, Roles.User.ToString());
            }

            var adminRole = await roleManager.FindByNameAsync(Roles.Admin.Name);
            await roleManager.AddPermissionClaim(adminRole, "Artist");
        }
    }

    private static async Task AddPermissionClaim(this RoleManager<Role> roleManager, Role role, string module)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        var allPermissions = Permissions.GeneratePermissionsForModule(module);
        foreach (var permission in allPermissions)
        {
            if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }
}