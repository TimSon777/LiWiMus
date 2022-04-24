module LiWiMus.API.Configuration.AutomapperCfg

open System.Reflection
open AutoMapper
open Microsoft.Extensions.DependencyInjection

type IServiceCollection with
    member services.AddMapper() =
        let mapperConfiguration = MapperConfiguration(fun cfg ->
            cfg.ShouldMapProperty <- fun p -> p.GetMethod |> isNull = false && p.GetMethod.IsPublic
            cfg.AddMaps(Assembly.GetExecutingAssembly()))
            
        services.AddTransient<IMapper>(fun _ -> mapperConfiguration.CreateMapper())
        