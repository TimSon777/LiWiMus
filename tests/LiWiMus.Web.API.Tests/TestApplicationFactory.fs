namespace LiWiMus.Web.API.Tests

open System
open LiWiMus.Core.Interfaces
open LiWiMus.Infrastructure.Data
open LiWiMus.Web.API.Tests.Mocks
open Microsoft.AspNetCore.Authorization.Policy
open Microsoft.AspNetCore.Mvc.Testing
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.DependencyInjection
open System.Linq

type TestApplicationFactory() =
    inherit WebApplicationFactory<Program>()
    override this.ConfigureWebHost(builder) =
        
        builder.ConfigureServices(fun services ->
            let applicationContextDescriptor = services.SingleOrDefault(fun d -> d.ServiceType = typeof<DbContextOptions<ApplicationContext>>)
            services.Remove(applicationContextDescriptor) |> ignore
 
            let databaseName = Guid.NewGuid().ToString()
            services.AddDbContext<ApplicationContext>(fun (options:DbContextOptionsBuilder) ->
                options.UseInMemoryDatabase(databaseName)
                |> ignore)
            |> ignore
            
            let avatarServiceDescriptor = services.SingleOrDefault(fun d -> d.ServiceType = typeof<IAvatarService>)
            services.Remove(avatarServiceDescriptor) |> ignore
            services.AddTransient<IAvatarService, MockAvatarService>() |> ignore
            
            services.AddSingleton<IPolicyEvaluator, MockPolicyEvaluator>() |> ignore)
        |> ignore

        base.ConfigureWebHost(builder)
        