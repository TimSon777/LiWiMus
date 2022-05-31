namespace LiWiMus.Web.API.Tests.Roles.Generators

open System
open Xunit

type CreateTestsGeneratorFailure() as this =
    inherit TheoryData<string, string>()
    do this.Add("", "Description")
    do this.Add("FailRole", "")
    do this.Add("FailRole1", null)
    do this.Add(null, "Description")
    do this.Add(String('x', 60), "Description")
    do this.Add("TestTest", String('x', 550))
    do this.Add("<Tags>", "Description")
    do this.Add("<>", "Description")
    do this.Add("A<Tags>", "Description")
    do this.Add("<Tags>A", "Description")
    do this.Add("A<Tags>A", "Description")
    do this.Add("Test234", "<Tags>")
    do this.Add("Test234ed", "<>")
    do this.Add("Test2vd34", "A<Tags>")
    do this.Add("Test2ce34", "<Tags>A")
    do this.Add("Test2wdc34", "A<Tags>A")