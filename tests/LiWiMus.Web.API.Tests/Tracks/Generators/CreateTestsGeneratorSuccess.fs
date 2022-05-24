namespace LiWiMus.Web.API.Tests.Tracks.Generators

open System
open System.Collections.Generic
open Xunit

type CreateTestsGeneratorSuccess() as this =
    inherit TheoryData<int, string, DateOnly, string, List<int>, List<int>, double>()
    do this.Add(220000, "TestTrackName", DateOnly(), "Location", List<int>([220000;220001]), List<int>([220000;220001]), 400)


