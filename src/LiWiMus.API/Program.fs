namespace LiWiMus.API

#nowarn "20"

open Autofac
open Autofac.Extensions.DependencyInjection
open LiWiMus.Core.Users
open LiWiMus.Infrastructure
open LiWiMus.Infrastructure.Data
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Identity
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open LiWiMus.API.Configuration.AutomapperCfg
open LiWiMus.Auth.Extensions.ServicesExtensions
open OpenIddict.Validation.AspNetCore
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

        services.AddAuthentication(fun options ->
            options.DefaultScheme <- OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
        
        services.AddAuthorization()
        
        services
            .AddOpenIddict()
            .AddValidation(fun options ->
                options.SetIssuer("https://localhost:5021") |> ignore
                options.UseSystemNetHttp() |> ignore
                options.UseAspNetCore() |> ignore)
            
        services.AddMapper()
        let app = builder.Build()

        app.UseRouting()

        app.UseAuthentication()
        app.UseAuthorization()
        
        app.UseEndpoints
            (fun endpoints ->
                endpoints.MapControllerRoute("default", "api/{controller}/{action}/{id?}")

                endpoints.MapControllerRoute("MyArea", "api/{area:exists}/{controller}/{action}/{id?}")
                |> ignore)

        app.Run()

        exitCode
