namespace LiWiMus.API

#nowarn "20"

open Autofac
open Autofac.Extensions.DependencyInjection
open LiWiMus.Infrastructure
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open LiWiMus.API.Configuration.AutomapperCfg

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)
        let services = builder.Services
        services.AddControllers()

        Dependencies.ConfigureServices(builder.Configuration, services)
        builder.Host
            .UseServiceProviderFactory(AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(fun (containerBuilder: ContainerBuilder) ->
                containerBuilder.RegisterModule(ConfigurationCoreModule(builder.Environment.ContentRootPath))
                |> ignore)

        services.AddMapper()
        let app = builder.Build()

        app.UseRouting()

        app.UseEndpoints
            (fun endpoints ->
                endpoints.MapControllerRoute("default", "api/{controller}/{action}/{id?}")

                endpoints.MapControllerRoute("MyArea", "api/{area:exists}/{controller}/{action}/{id?}")
                |> ignore)

        app.Run()

        exitCode
