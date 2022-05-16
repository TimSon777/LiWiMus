module LiWiMus.Web.API.Tests.WebApplicationFactory

open LiWiMus.Infrastructure.Data
open Microsoft.AspNetCore.Mvc.Testing
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.TestHost
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging

type TestApplicationFactory() =
    inherit WebApplicationFactory<Program>()

    override this.CreateHostBuilder() =
        Host
            .CreateDefaultBuilder()
            .ConfigureWebHostDefaults(fun builder ->
                builder
                    .UseStartup<Program>()
                    .UseTestServer()
                |> ignore)
        |> ignore

        base.CreateHostBuilder()

    override this.ConfigureWebHost(builder) =
        builder
            .UseEnvironment("Testing")
            .ConfigureServices(fun services ->
                let serviceProvider = services.BuildServiceProvider()
                let scope = serviceProvider.CreateScope()
                let scopedServices = scope.ServiceProvider

                let db =
                    scopedServices.GetRequiredService<ApplicationContext>()

                let logger =
                    scopedServices.GetRequiredService<ILogger<TestApplicationFactory>>()

                db.Database.EnsureDeleted()
                |> ignore)
        |> ignore

        base.ConfigureWebHost(builder)
