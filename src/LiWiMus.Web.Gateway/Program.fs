namespace LiWiMus.Web.Gateway

#nowarn "20"

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging


open Ocelot.DependencyInjection
open Ocelot.Middleware

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =
        let builder =
            WebApplication.CreateBuilder(args)

        builder.Logging.AddSimpleConsole()

        let configuration = build
        let configuration = builder.Configuration
        let services = builder.Services

        let ocelot =
            if builder.Environment.IsDevelopment() then
                "configuration.Development.json"
            else
                "configuration.json"

        configuration.AddJsonFile(ocelot)

        services.AddOcelot()
        if builder.Environment.IsDevelopment() then
            services.AddCors (fun options ->
                options.AddDefaultPolicy (fun builder ->
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                    |> ignore))
        else
            services.AddCors()

        let app = builder.Build()

        //app.UseHttpsRedirection()
        app.UseCors()

        app.UseOcelot().Wait()

        app.Run()

        exitCode