namespace LiWiMus.Web.API.Tests

open System
open LiWiMus.Infrastructure.Data
open LiWiMus.Web.API.Tests.MockPolicyEvaluator
open Microsoft.AspNetCore.Authorization.Policy
open Microsoft.AspNetCore.Mvc.Testing
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.DependencyInjection
open System.Linq

type TestApplicationFactory() =
    inherit WebApplicationFactory<Program>()

    override this.ConfigureWebHost(builder) =
        
        builder.ConfigureServices(fun services ->
            let descriptor = services.SingleOrDefault(fun d -> d.ServiceType = typeof<DbContextOptions<ApplicationContext>>);
            services.Remove(descriptor) |> ignore
 
            let databaseName = Guid.NewGuid().ToString()
            services.AddDbContext<ApplicationContext>(fun (options:DbContextOptionsBuilder) ->
                options.UseInMemoryDatabase(databaseName)
                |> ignore)
            |> ignore
            
            let sp = services.BuildServiceProvider()
            use scope = sp.CreateScope()
            services.AddSingleton<IPolicyEvaluator, MockPolicyEvaluator>() |> ignore)
        |> ignore

        base.ConfigureWebHost(builder)
       
        