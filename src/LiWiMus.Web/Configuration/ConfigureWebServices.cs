using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Settings;
using LiWiMus.Web.Permission;
using LiWiMus.Web.Services;
using Microsoft.AspNetCore.Authorization;

namespace LiWiMus.Web.Configuration;

public static class ConfigureWebServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DataSettings>(configuration.GetSection("DataSettings"));
        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        services.AddTransient<IRazorViewRenderer, RazorViewRenderer>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, AuthorizationHandler>();
        return services;
    }
}