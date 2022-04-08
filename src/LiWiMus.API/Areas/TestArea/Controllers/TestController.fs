namespace LiWiMus.API.Controllers

open System
open Microsoft.AspNetCore.Mvc

[<Area("TestArea")>]
type TestController () =
    inherit ControllerBase()

    [<HttpGet>]
    member _.TestGet() =
        Random.Shared.Next 5      