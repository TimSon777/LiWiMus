namespace LiWiMus.Auth.Controllers

open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open OpenIddict.Validation.AspNetCore

type TestController () =
    inherit Controller()
    
    [<HttpGet>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    member _.Get1() =
        let rng = System.Random()
        rng.Next(0,12)
