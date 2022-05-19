using LiWiMus.Infrastructure.Data.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LiWiMus.Infrastructure.Data;

public static class ApplicationContextSeed
{
    public static async Task SeedAsync(ApplicationContext applicationContext, 
        EnvironmentType environmentType,
        ILogger logger, 
        ISeeder[] seeders,
        int retry = 0)
    {
        var retryForAvailability = retry;
        try
        {
            if (applicationContext.Database.IsMySql())
            {
                await applicationContext.Database.MigrateAsync();
            }

            foreach (var seeder in seeders.OrderByDescending(x => x.Priority))
            {
                await seeder.SeedAsync(environmentType);
            }

        }
        catch (Exception ex)
        {
            //File.WriteAllText(@"C:\Users\Тимур\Desktop\LiWiMus\src\LiWiMus.Web.API/EXCEPTION" + Guid.NewGuid().ToString()[..10] + ".txt", ex.Message + "\n" + ex.StackTrace);
            if (retryForAvailability >= 10)
            {
                throw;
            }

            retryForAvailability++;

            logger.LogError("Error while seeding database: {Message}", ex.Message);
            await SeedAsync(applicationContext, environmentType, logger, seeders, retryForAvailability);
            throw;
        }
    }
}