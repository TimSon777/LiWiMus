namespace LiWiMus.Web.API.Tests.Users.Generators

open System
open Xunit

type LockoutTestsGeneratorSuccess() as this =
    inherit TheoryData<DateTime>()
    do this.Add(DateTime.MinValue)
    do this.Add(DateTime.UtcNow)
    do this.Add(DateTime(2000, 10, 10))
