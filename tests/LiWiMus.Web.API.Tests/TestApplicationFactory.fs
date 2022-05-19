module LiWiMus.Web.API.Tests.WebApplicationFactory

open LiWiMus.Infrastructure.Data
open Microsoft.AspNetCore.Mvc.Testing
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection

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
        