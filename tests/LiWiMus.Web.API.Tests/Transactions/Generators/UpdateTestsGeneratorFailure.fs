namespace LiWiMus.Web.API.Tests.Transactions.Generators

open System
open Microsoft.FSharp.Core
open Xunit

type UpdateTestsGeneratorFailure() as this =
    inherit TheoryData<string>()
    do this.Add(String('x', 150))
    do this.Add(String('x', 170))
    do this.Add(String('x', 120))