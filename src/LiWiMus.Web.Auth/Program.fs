namespace LiWiMus.Web.Auth

#nowarn "20"

open System
open System.Collections.Generic
open System.IdentityModel.Tokens.Jwt
open LiWiMus.Infrastructure.Data.Config
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open LiWiMus.Web.Auth.Extensions.ServicesExtensions
open LiWiMus.Web.Shared.Extensions
open Microsoft.Net.Http.Headers
open Microsoft.OpenApi.Models


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

        services.AddDbContext(configuration)

        services
            .AddIdentity()
            .AddOpenIdConnect()
            
        let info = OpenApiInfo()
        info.Title <- builder.Environment.ApplicationName
        
        let scheme = OpenApiSecurityScheme()
        scheme.Name           <- HeaderNames.Authorization
        scheme.Type           <- SecuritySchemeType.OAuth2
        scheme.Scheme         <- JwtBearerDefaults.AuthenticationScheme
        scheme.BearerFormat   <- JwtConstants.TokenType
        scheme.In             <- ParameterLocation.Header
        
        let flows = OpenApiOAuthFlows()
        let passwordFlow = OpenApiOAuthFlow()
        passwordFlow.TokenUrl <- Uri("https://localhost:5021/auth/connect/token")
        flows.Password <- passwordFlow
        scheme.Flows <- flows
        
        let securityRequirement = OpenApiSecurityRequirement()
        let securityScheme = OpenApiSecurityScheme()
        let reference = OpenApiReference()
        reference.Type <- ReferenceType.SecurityScheme
        reference.Id <- "oauth2"
        securityScheme.Reference <- reference
        securityRequirement.Add(securityScheme, Array.empty)
        
        services.AddSwaggerGen(fun o ->
            o.SwaggerDoc("v1", info)
            o.AddSecurityDefinition("oauth2", scheme)
            o.AddSecurityRequirement(securityRequirement)) |> ignore
        
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