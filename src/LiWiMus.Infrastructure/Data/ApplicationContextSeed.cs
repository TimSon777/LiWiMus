using LiWiMus.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LiWiMus.Infrastructure.Data;

public class ApplicationContextSeed
{
    public static async Task SeedAsync(ApplicationContext applicationContext,
                                       ILogger logger,
                                       int retry = 0)
    {
        var retryForAvailability = retry;
        try
        {
            if (applicationContext.Database.IsMySql())
            {
                applicationContext.Database.Migrate();
            }

            if (!await applicationContext.Plans.AnyAsync())
            {
                await applicationContext.Plans.AddRangeAsync(
                    GetPreconfiguredPlans());

                await applicationContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            if (retryForAvailability >= 10) throw;

            retryForAvailability++;

            logger.LogError(ex.Message);
            await SeedAsync(applicationContext, logger, retryForAvailability);
            throw;
        }
    }

    private static IEnumerable<Plan> GetPreconfiguredPlans()
    {
        return new List<Plan>
        {
            new()
            {
                Name = "Free",
                Description = "Just free plan.",
                PricePerMonth = 0
            },
            new()
            {
                Name = "Premium",
                Description = "Premium plan.",
                PricePerMonth = 100
            }
        };
    }
}