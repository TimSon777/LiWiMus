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

    override this.ConfigureWebHost(builder) =
        builder
            .UseEnvironment("Testing")
        |> ignore

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
        