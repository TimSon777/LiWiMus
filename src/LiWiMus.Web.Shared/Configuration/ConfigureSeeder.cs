using LiWiMus.Infrastructure.Data.Seeders;
using Microsoft.Extensions.DependencyInjection;

namespace LiWiMus.Web.Shared.Configuration;

public static class ConfigureSeeder
{
    public static void AddSeeders(this IServiceCollection services)
    {
        var seederInterface = typeof(ISeeder);
        var seeders = seederInterface
            .Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(seederInterface) && !t.IsInterface);
        
        foreach (var seeder in seeders)
        {
            services.AddScoped(seederInterface, seeder);
        }
    }
}