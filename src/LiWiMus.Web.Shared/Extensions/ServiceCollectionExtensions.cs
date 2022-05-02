#region

using LiWiMus.Core.Settings;
using LiWiMus.Web.Shared.Services;
using LiWiMus.Web.Shared.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace LiWiMus.Web.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        services.AddTransient<IFormFileSaver, FormFileSaver>();

        return services;
    }

    public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSection = configuration.GetRequiredSection(DataSettings.ConfigName);
        services.Configure<DataSettings>(dataSection);
        var dataSettings = dataSection.Get<DataSettings>();
        dataSettings.CreateDirectories();

        services.Configure<MailSettings>(configuration.GetSection(MailSettings.ConfigName));

        return services;
    }
}