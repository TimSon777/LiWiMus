#region

using LiWiMus.Core.Settings;
using LiWiMus.SharedKernel.Helpers;
using LiWiMus.Web.Shared.Services;
using LiWiMus.Web.Shared.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
        services.Configure<SharedSettings>(configuration);
        services.PostConfigure<SharedSettings>(settings =>
        {
            settings.SharedDirectory = PathHelper.ReplaceWithDirectorySeparatorChar(settings.SharedDirectory);

            var relativeDataDir = PathHelper.ReplaceWithDirectorySeparatorChar(settings.DataSettings.DataDirectory);
            settings.DataSettings.MusicDirectory = Path.Combine(relativeDataDir, DataSettings.MusicDirectoryName);
            settings.DataSettings.PicturesDirectory = Path.Combine(relativeDataDir, DataSettings.PicturesDirectoryName);
        });

        var settings = services.BuildServiceProvider().GetRequiredService<IOptions<SharedSettings>>().Value;

        services.Configure<DataSettings>(dataSettings =>
        {
            dataSettings.DataDirectory = settings.DataSettings.DataDirectory;
            dataSettings.MusicDirectory = settings.DataSettings.MusicDirectory;
            dataSettings.PicturesDirectory = settings.DataSettings.PicturesDirectory;
        });
        services.PostConfigure<DataSettings>(dataSettings => dataSettings.CreateDirectories());

        services.Configure<MailSettings>(configuration.GetSection(MailSettings.ConfigName));

        return services;
    }
}