using LiWiMus.Infrastructure.Data;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Web.Configuration;

public static class ConfigureCoreServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        return services;
    }
}