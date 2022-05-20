namespace LiWiMus.Web.API.Tests

open LiWiMus.Infrastructure.Data
open LiWiMus.Web.Shared.Configuration
open Microsoft.AspNetCore.Mvc.Testing
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging

type BaseApplicationFactory<'TStartup when 'TStartup : not struct>() =
    inherit WebApplicationFactory<'TStartup>()

    override this.ConfigureWebHost(builder) =
        builder
            .UseEnvironment("Testing")
        |> ignore
        
        builder.ConfigureServices(fun services ->
            let sp = services.BuildServiceProvider()
            let scope = sp.CreateScope()
            let logger = scope.ServiceProvider.GetRequiredService<ILogger<BaseApplicationFactory<'TStartup>>>()
            let env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>()
            
            ConfigureSeeder.UseSeedersAsync(scope.ServiceProvider, logger, env).Wait()
            scope.Dispose()) |> ignore

        base.ConfigureWebHost(builder)
        
    override this.DisposeAsync() =
        let scope = this.Server.Services.CreateScope()
        let scopedServices = scope.ServiceProvider

        let db =
            scopedServices.GetRequiredService<ApplicationContext>()

        task {
            return! db.Database.EnsureDeletedAsync()
        } |> ignore
        
        base.DisposeAsync()
        