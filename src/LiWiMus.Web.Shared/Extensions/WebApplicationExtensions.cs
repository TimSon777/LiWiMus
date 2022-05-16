using LiWiMus.Infrastructure;
using LiWiMus.Infrastructure.Data;
using LiWiMus.Infrastructure.Data.Seeders;
using Microsoft.AspNetCore.Builder;
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
            var envType = Enum.Parse<EnvironmentType>(app.Environment.EnvironmentName);
            await ApplicationContextSeed.SeedAsync(applicationContext, envType, logger, scopedProvider);
            await ApplicationContextClear.ClearAsync(applicationContext, logger, app.Environment.IsDevelopment());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the DB");
        }
    }
}