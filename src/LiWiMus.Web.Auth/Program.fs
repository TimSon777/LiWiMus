namespace LiWiMus.Web.Auth

#nowarn "20"

open LiWiMus.Infrastructure
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open LiWiMus.Web.Auth.Extensions.ServicesExtensions
open LiWiMus.Web.Shared.Extensions


module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder =
            WebApplication.CreateBuilder(args)

        let services = builder.Services
        let environment = builder.Environment
        let configuration = builder.Configuration

        builder.Configuration.AddSharedSettings(builder.Environment)

        services.AddControllers()

        if environment.IsDevelopment() then
            services.AddCors (fun options ->
                options.AddDefaultPolicy (fun builder ->
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                    |> ignore))
        else
            services.AddCors()

        services
            .AddAuthenticationWithJwt()
            .AddAuthorization()

        Dependencies.ConfigureServices(configuration, services)

        services.AddIdentity().AddOpenIdConnect()

        let app = builder.Build()

        if environment.IsDevelopment() then
            app.UseDeveloperExceptionPage() |> ignore

        app
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseCors()

        app.UseEndpoints (fun endpoints ->
            endpoints.MapControllerRoute("default", "/auth/{controller}/{action}/{id?}")
            |> ignore)

        app.MapControllers()

        app.Run()

        exitCode