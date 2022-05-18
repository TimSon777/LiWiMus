namespace LiWiMus.Web.Auth

#nowarn "20"

open LiWiMus.Infrastructure.Data.Config
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open LiWiMus.Web.Auth.Extensions.ServicesExtensions
open LiWiMus.Web.Shared.Configuration
open Microsoft.Extensions.Configuration

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder =
            WebApplication.CreateBuilder(args)

        let services = builder.Services
        let environment = builder.Environment
        let configuration = builder.Configuration.GetConnectionString("DefaultConnection")

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

        services
            .AddDbContext(configuration)

        services
            .AddIdentity()
            .AddOpenIdConnect()
            .AddSwaggerWithAuthorize(builder.Environment.ApplicationName)
        
        let app = builder.Build()

        if environment.IsDevelopment() then
            app.UseDeveloperExceptionPage() |> ignore

        app
            .UseRouting()
            .UseCors()
            .UseAuthentication()
            .UseAuthorization()

        app.UseEndpoints (fun endpoints ->
            endpoints.MapControllerRoute("default", "/auth/{controller}/{action}/{id?}")
            |> ignore)

        app.UseSwagger()
        app.UseSwaggerUI(fun c ->
            c.SwaggerEndpoint("/swagger/v1/swagger.json", builder.Environment.ApplicationName)
            c.RoutePrefix <- "api/auth/swagger")
        
        app.MapControllers()

        app.Run()

        exitCode