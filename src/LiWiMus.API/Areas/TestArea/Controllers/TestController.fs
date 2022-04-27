namespace LiWiMus.API.Controllers

open System
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Server.HttpSys
open OpenIddict.Validation.AspNetCore

[<Area("TestArea")>]
type TestController () =
    inherit ControllerBase()

    [<HttpGet>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    member _.TestGet() =
        Random.Shared.Next 5      