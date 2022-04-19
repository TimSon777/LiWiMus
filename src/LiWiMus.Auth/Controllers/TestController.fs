namespace LiWiMus.Auth.Controllers

open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open OpenIddict.Validation.AspNetCore

type TestController () =
    inherit Controller()
    
    [<HttpGet("tests")>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    member _.Get() =
        let rng = System.Random()
        rng.Next(0,12)
