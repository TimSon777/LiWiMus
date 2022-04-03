namespace LiWiMus.API

#nowarn "20"

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        let services = builder.Services
        services.AddControllers()

        let app = builder.Build()

        app.UseHttpsRedirection()

        app.UseRouting()

        app.UseEndpoints
            (fun endpoints ->
                endpoints.MapControllerRoute("default", "api/{controller}/{action}/{id?}")
                endpoints.MapControllerRoute("MyArea", "api/{area:exists}/{controller}/{action}/{id?}")
                |> ignore)

        app.Run()

        exitCode
