namespace LiWiMus.Web.API.Tests.Tracks.Generators

open System
open System.Collections.Generic
open Xunit

type CreateTestsGeneratorFailure() as this =
    inherit TheoryData<int, string, DateOnly, string, List<int>, List<int>, double>()
    do this.Add(220000, "T", DateOnly(), "Location", List<int>([220000;220001]), List<int>([220000;220001]), 400)
    do this.Add(220000, String('x', 60), DateOnly(), "Location", List<int>([220000;220001]), List<int>([220000;220001]), 400)
    do this.Add(220000, "<Tags>", DateOnly(), "Location", List<int>([220000;220001]), List<int>([220000;220001]), 400)
    do this.Add(220000, "A<Tags>", DateOnly(), "Location", List<int>([220000;220001]), List<int>([220000;220001]), 400)
    do this.Add(220000, "<Tags>A", DateOnly(), "Location", List<int>([220000;220001]), List<int>([220000;220001]), 400)
    do this.Add(220000, "A<Tags>A", DateOnly(), "Location", List<int>([220000;220001]), List<int>([220000;220001]), 400)
    do this.Add(220000, "TestName", DateOnly(), "Location", List<int>(), List<int>([220000;220001]), 400)
    do this.Add(220000, "TestName", DateOnly(), "Location", List<int>([220000;220001]), List<int>(), 400)
    do this.Add(220000, "TestName", DateOnly.MaxValue, "Location", List<int>([220000;220001]), List<int>([220000;220001]), 400)
    do this.Add(220000, "TestName", DateOnly(), "Location", List<int>([220000;220001]), List<int>([220000;220001]), 0)
    do this.Add(220000, "TestName", DateOnly(), "Location", List<int>([220000;220001]), List<int>([220000;220001]), -10)
    do this.Add(220000, "TestName", DateOnly(), "", List<int>([220000;220001]), List<int>([220000;220001]), 10)
    do this.Add(220000, "TestName", DateOnly(), null, List<int>([220000;220001]), List<int>([220000;220001]), 10)
    do this.Add(220000, null, DateOnly(), "Location", List<int>([220000;220001]), List<int>([220000;220001]), 10)
    do this.Add(220000, "TestName", DateOnly(), String('x', 120), List<int>([220000;220001]), List<int>([220000;220001]), 10)