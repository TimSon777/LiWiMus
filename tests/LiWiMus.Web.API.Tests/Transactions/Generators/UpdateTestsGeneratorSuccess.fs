namespace LiWiMus.Web.API.Tests.Transactions.Generators

open System
open Microsoft.FSharp.Core
open Xunit

type UpdateTestsGeneratorSuccess() as this =
    inherit TheoryData<decimal>()
    do this.Add(1000)
    do this.Add(-100)
    do this.Add(10.5M)
    do this.Add(444)
    do this.Add(-223)
