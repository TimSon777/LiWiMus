namespace LiWiMus.Web.Auth.Controllers.TestController

open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Components
open Microsoft.AspNetCore.Mvc
open OpenIddict.Validation.AspNetCore

[<ApiController>]
[<Route("Hello")>]
type TestController() =
    inherit ControllerBase()
    
    [<HttpGet("Buy")>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    member this.Token() =
        0