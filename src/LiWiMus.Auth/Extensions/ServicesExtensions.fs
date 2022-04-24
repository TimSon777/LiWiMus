module LiWiMus.Auth.Extensions.ServicesExtensions

open LiWiMus.Core.Roles
open LiWiMus.Core.Users
open LiWiMus.Infrastructure.Data
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Identity
open OpenIddict.Abstractions;

type IServiceCollection with 
    member services.AddAuthenticationWithJwt() =
            services
                .AddAuthentication(fun options ->
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme |> ignore
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme |> ignore)
                .AddJwtBearer(fun options ->
                    options.ClaimsIssuer = JwtBearerDefaults.AuthenticationScheme |> ignore)
            |> ignore
            services
    
    member services.AddIdentity() =
            services
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders()
            |> ignore
    
            services.Configure<IdentityOptions>(fun (options:IdentityOptions) ->
                options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name |> ignore
                options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject |> ignore
                options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role |> ignore
                options.ClaimsIdentity.EmailClaimType = OpenIddictConstants.Claims.Email |> ignore)
            |> ignore
                
            services
    
    member services.AddOpenIdConnect() =
            services
                .AddOpenIddict()
                .AddCore(fun options ->
                    options
                        .UseEntityFrameworkCore()
                        .UseDbContext<ApplicationContext>()
                    |> ignore)
                .AddServer(fun options ->
                    options
                        .AcceptAnonymousClients()
                        .AllowPasswordFlow()
                        .AllowRefreshTokenFlow()
                        .SetTokenEndpointUris("/connect/token")
                        .UseAspNetCore(fun builder ->
                            builder
                                .EnableTokenEndpointPassthrough()
                            |> ignore)
                        .AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate()
                    |> ignore)
                .AddValidation(fun options ->
                    options.UseAspNetCore() |> ignore
                    options.UseLocalServer() |> ignore)
            |> ignore
        
            services