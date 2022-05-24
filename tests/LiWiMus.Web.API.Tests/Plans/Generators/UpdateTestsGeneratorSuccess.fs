namespace LiWiMus.Web.API.Tests.Plans.Generators

open System
open Microsoft.FSharp.Core
open Xunit

type UpdateTestsGeneratorSuccess() as this =
    inherit TheoryData<int, string, Nullable<decimal>>()
    do this.Add(180000, null, Nullable())
    do this.Add(180000, "Description", Nullable())
    do this.Add(180000, null, 30)
    do this.Add(180000, null, 26.56M)
    do this.Add(180000, null, 0.1M)
    do this.Add(180000, "Description", 100)
