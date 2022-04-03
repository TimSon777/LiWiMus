namespace LiWiMus.API.Controllers

open System
open Microsoft.AspNetCore.Mvc

type TestWithoutAreaController () =
    inherit ControllerBase()

    [<HttpGet>]
    member _.TestGet() =
        Random.Shared.Next 5   