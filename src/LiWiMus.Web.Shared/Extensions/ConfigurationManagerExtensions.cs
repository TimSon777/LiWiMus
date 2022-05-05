using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace LiWiMus.Web.Shared.Extensions;

public static class ConfigurationManagerExtensions
{
    public static ConfigurationManager AddSharedSettings(this ConfigurationManager configurationBuilder,
                                                         IWebHostEnvironment environment)
    {
        var contentRoot = environment.ContentRootPath;

        var sharedSettingsDir = configurationBuilder.GetRequiredSection("SharedDirectory").Get<string>()
                                                    .Replace(':', Path.DirectorySeparatorChar);
        var sharedSettingsPaths = new[]
        {
            Path.Combine(sharedSettingsDir, "sharedsettings.json"),
            Path.Combine(sharedSettingsDir, $"sharedsettings.{environment.EnvironmentName}.json")
        };

        foreach (var path in sharedSettingsPaths)
        {
            var absPath = Path.Combine(contentRoot, path);
            configurationBuilder.AddJsonFile(absPath, true);
        }

        return configurationBuilder;
    }
}