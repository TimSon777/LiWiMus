using LiWiMus.Core.Interfaces;
using LiWiMus.Infrastructure.Data;
using LiWiMus.Infrastructure.Services;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Web.Configuration;

public static class ConfigureCoreServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        services.AddSingleton<IAvatarService, AvatarService>();

        services.AddTransient<IMailService, MailService>();
        services.AddTransient<IMailRequestService, MailRequestService>();

        return services;
    }
}