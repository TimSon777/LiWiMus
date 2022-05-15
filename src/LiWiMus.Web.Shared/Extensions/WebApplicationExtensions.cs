using LiWiMus.Core.Settings;
using LiWiMus.Core.Users;
using LiWiMus.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LiWiMus.Web.Shared.Extensions;

public static class WebApplicationExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app, ILogger logger)
    {
        logger.LogInformation("Seeding Database...");

        using var scope = app.Services.CreateScope();
        var scopedProvider = scope.ServiceProvider;

        try
        {
            var applicationContext = scopedProvider.GetRequiredService<ApplicationContext>();
            var userManager = scopedProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var adminSettings = app.Configuration.GetSection("AdminSettings").Get<AdminSettings>();
            await ApplicationContextSeed.SeedAsync(applicationContext, logger, userManager, roleManager, adminSettings);
            await ApplicationContextClear.ClearAsync(applicationContext, logger, app.Environment.IsDevelopment());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the DB");
        }
    }
}