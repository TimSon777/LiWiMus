namespace LiWiMus.Web.Gateway

#nowarn "20"

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open LiWiMus.Web.Shared.Configuration
open Ocelot.DependencyInjection
open Ocelot.Middleware

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =
        let builder =
            WebApplication.CreateBuilder(args)

        builder.Logging.AddSimpleConsole()

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

        services.AddControllers()
        services.AddSwaggerWithAuthorize(builder.Environment.ApplicationName)
        services.AddSwaggerForOcelot(configuration)
        
        let app = builder.Build()

        if app.Environment.IsDevelopment() then
            app.UseDeveloperExceptionPage() |> ignore
        //app.UseHttpsRedirection()
        app.UseCors()

        app.UseSwagger()
        app.UseSwaggerForOcelotUI(fun o ->
            o.PathToSwaggerGenerator <- "/swagger/docs"
            o.SwaggerEndpoint("/swagger/v1/swagger.json", builder.Environment.ApplicationName))
        
        app
            .UseOcelot()
            .Wait()

        app.MapControllers()
        app.Run()

        exitCode