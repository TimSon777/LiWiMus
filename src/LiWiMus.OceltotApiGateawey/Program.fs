namespace LiWiMus.OceltotApiGateawey

#nowarn "20"

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Ocelot.DependencyInjection
open Ocelot.Middleware

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)
        let ocelot =
            if builder.Environment.IsDevelopment() then
                "configuration.Development.json"
            else
                "configuration.json"
                
        builder.Configuration.AddJsonFile(ocelot)
        builder.Services.AddOcelot()

        let app = builder.Build()

        app.UseHttpsRedirection()
        app.UseOcelot()
        app.Run()

        exitCode