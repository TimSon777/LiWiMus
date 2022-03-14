using LiWiMus.Core;
using LiWiMus.Core.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace LiWiMus.Web.Configuration;

public static class ConfigureWebServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DataSettings>(configuration.GetSection("DataSettings"));
        return services;
    }
}