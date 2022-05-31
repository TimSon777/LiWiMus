namespace LiWiMus.Web.Auth

#nowarn "20"

open LiWiMus.Core.Settings
open LiWiMus.Infrastructure.Data.Config
open LiWiMus.Infrastructure.Data.Seeders
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open LiWiMus.Web.Auth.Extensions.ServicesExtensions
open LiWiMus.Web.Shared.Configuration
open Microsoft.Extensions.Configuration

type Program() =
    static let exitCode = 0

    [<EntryPoint>]
    static let main args =

        let builder =
            WebApplication.CreateBuilder(args)

        let services = builder.Services
        let environment = builder.Environment
        let connection = builder.Configuration
                                .GetConnectionString("DefaultConnection")

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
            .AddDbContext(connection)

        services.AddRepositoriesAndManagers()

        services
            .AddIdentity(builder.Environment)
            .ConfigureIdentityOptions()
            .AddOpenIdConnect(builder.Configuration)
            .AddSwaggerWithAuthorize(builder.Environment.ApplicationName)
            
        services
            .Configure<AdminSettings>(builder.Configuration.GetSection(nameof(AdminSettings)))
            
        services
            .AddScoped<ISeeder, UserSeeder>()
        
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
        
        ConfigureSeeder.UseSeedersAsync(app.Services, app.Logger, builder.Environment).Wait()
        
        app.Run()

        exitCode