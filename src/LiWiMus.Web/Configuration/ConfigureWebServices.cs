using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Settings;
using LiWiMus.Web.Services;

namespace LiWiMus.Web.Configuration;

public static class ConfigureWebServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DataSettings>(configuration.GetSection("DataSettings"));
        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        services.AddTransient<IRazorViewRenderer, RazorViewRenderer>();
        return services;
    }
}