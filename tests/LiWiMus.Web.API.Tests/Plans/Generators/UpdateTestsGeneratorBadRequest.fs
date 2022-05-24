namespace LiWiMus.Web.API.Tests.Plans.Generators

open System
open Microsoft.FSharp.Core
open Xunit

type UpdateTestsGeneratorBadRequest() as this =
    inherit TheoryData<int, string, Nullable<decimal>>()
    do this.Add(180000, null, -0.1M)
    do this.Add(180000, "Description", -100)
    do this.Add(180000, "<Tags>", 100)
    do this.Add(180000, "A<Tags>A", 100)
    do this.Add(180000, "A<Tags>", 100)
    do this.Add(180000, "<Tags>A", 100)
