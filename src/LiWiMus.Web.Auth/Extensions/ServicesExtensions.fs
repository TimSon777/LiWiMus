module LiWiMus.Web.Auth.Extensions.ServicesExtensions

open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Identity
open OpenIddict.Abstractions
open LiWiMus.Core.Users
open LiWiMus.Infrastructure.Data

type IServiceCollection with
    member public services.AddAuthenticationWithJwt() =
        services
            .AddAuthentication(fun options ->
                options.DefaultAuthenticateScheme <- JwtBearerDefaults.AuthenticationScheme
                options.DefaultChallengeScheme <- JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(fun options -> options.ClaimsIssuer <- JwtBearerDefaults.AuthenticationScheme)
        |> ignore

        services

    member public services.AddIdentity() =
        services
            .AddIdentity<User, IdentityRole<int>>(fun o ->
                o.Password.RequireDigit <- false
                o.Password.RequiredLength <- 3
                o.Password.RequireLowercase <- false
                o.Password.RequireUppercase <- false
                o.Password.RequireNonAlphanumeric <- false)
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders()
        |> ignore

        services.Configure<IdentityOptions> (fun (options: IdentityOptions) ->
            options.ClaimsIdentity.UserNameClaimType <- OpenIddictConstants.Claims.Name
            options.ClaimsIdentity.UserIdClaimType <- OpenIddictConstants.Claims.Subject
            options.ClaimsIdentity.RoleClaimType <- OpenIddictConstants.Claims.Role
            options.ClaimsIdentity.EmailClaimType <- OpenIddictConstants.Claims.Email)
        |> ignore

        services

    member public services.AddOpenIdConnect() =
        services
            .AddOpenIddict()
            .AddCore(fun options ->
                options
                    .UseEntityFrameworkCore()
                    .UseDbContext<ApplicationContext>()
                |> ignore)
            .AddServer(fun options ->
                options
                    .AllowPasswordFlow()
                    .SetTokenEndpointUris("/auth/connect/token")
                    .AcceptAnonymousClients()
                    .UseAspNetCore(fun builder ->
                        builder
                            .DisableTransportSecurityRequirement()
                            .EnableTokenEndpointPassthrough()
                        |> ignore)

                    .AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate()
                    .DisableAccessTokenEncryption()
                |> ignore)
            .AddValidation(fun options ->
                options.UseAspNetCore() |> ignore
                options.UseLocalServer() |> ignore)
        |> ignore

        services
