using LiWiMus.Infrastructure.Data.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LiWiMus.Infrastructure.Data;

public static class ApplicationContextSeed
{
    public static async Task SeedAsync(ApplicationContext applicationContext, 
        EnvironmentType environmentType,
        ILogger logger, 
        IServiceProvider serviceProvider, 
        int retry = 0)
    {
        var retryForAvailability = retry;
        try
        {
            if (applicationContext.Database.IsMySql())
            {
                await applicationContext.Database.MigrateAsync();
            }

            var seeders = serviceProvider
                .GetServices<ISeeder>()
                .OrderByDescending(s => s.Priority);

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(environmentType);
            }

        }
        catch (Exception ex)
        {
            if (retryForAvailability >= 10)
            {
                throw;
            }

            retryForAvailability++;

            logger.LogError("Error while seeding database: {Message}", ex.Message);
            await SeedAsync(applicationContext, environmentType, logger, serviceProvider, retryForAvailability);
            throw;
        }
    }
}