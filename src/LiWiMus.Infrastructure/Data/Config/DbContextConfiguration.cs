using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiWiMus.Infrastructure.Data.Config;

public static class DbContextConfiguration
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options
                .UseLazyLoadingProxies()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                    options => options.EnableRetryOnFailure())
                .UseValidationCheckConstraints()
                .UseAllCheckConstraints()
                .UseOpenIddict();
        });
    }
}