using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LiWiMus.Infrastructure.Data.Config;

public static class DbContextConfiguration
{
    public static void AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationContext>(options =>
        {
            options
                .UseLazyLoadingProxies()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                    options => options.EnableRetryOnFailure())
                .UseValidationCheckConstraints()
                .UseAllCheckConstraints()
                .UseOpenIddict()
                .EnableSensitiveDataLogging();
        });
    }
}