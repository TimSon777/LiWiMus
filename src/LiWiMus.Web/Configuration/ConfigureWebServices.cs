using LiWiMus.Core.Entities;
using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Settings;
using LiWiMus.Infrastructure.Identity;
using LiWiMus.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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
        services.AddTransient<IUserValidator<User>, ApplicationUserValidator>();
        return services;
    }
}