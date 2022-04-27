namespace LiWiMus.API.Controllers

open System
open LiWiMus.API.Infrastructure.Attributes
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Server.HttpSys
open OpenIddict.Validation.AspNetCore

[<Area("TestArea")>]
type TestController () =
    inherit ControllerBase()

    [<HttpGet>]
    [<AuthorizeWithJwt>]
    member _.TestGet() =
        Random.Shared.Next 5      