using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace LiWiMus.Web.Shared.Configuration;

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