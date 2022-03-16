using System.Reflection;
using AutoMapper;

namespace LiWiMus.Web.Configuration;

public static class ConfigureMapper
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.ShouldMapProperty = p => p.GetMethod != null && p.GetMethod.IsPublic;
            cfg.AddMaps(Assembly.GetExecutingAssembly());
        });

        return services.AddTransient<IMapper>(_ => mapperConfiguration.CreateMapper());
    }
}