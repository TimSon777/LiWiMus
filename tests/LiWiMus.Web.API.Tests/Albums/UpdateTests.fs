namespace LiWiMus.Web.API.Tests.Albums

open System
open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open System.Net.Http.Json
open FluentAssertions
open Microsoft.AspNetCore.Http.Json
open LiWiMus.Web.API.Tests.JsonOptionsExtensions
open LiWiMus.Web.Shared.Extensions

type UpdateTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.Albums.Update
        interface IClassFixture<TestApplicationFactory>
    
        [<Theory>]
        [<InlineData("AlbumTest", "Location")>]
        [<InlineData("Longgggggggggggggggggggggggggggggg", "Longgggggggggggggggggggggggggggggg")>]
        [<InlineData(null, null)>]
        member this.``Tests(Albums): Update => Success``(title, coverLocation) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Albums.Update.Request(
                Id = 90000,
                Title = title,
                CoverLocation = coverLocation,
                PublishedAt = DateOnly())
            
            task {
                
                //Act
                let! httpMessage = client.PatchAsJsonAsync(url, body, JsonOptions().WithDateTimeOnly())
                
                //Assert
                httpMessage
                    .Should()
                    .BeSuccessful("all params must be valid (see Validator)")
                    |> ignore
            }
            
        [<Theory>]
        [<InlineData("X", "X")>]
        [<InlineData("VeryLonggggggggggggggggggggggggggggggggggggggggggggggggggggggg", "Longggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg")>]
        member this.``Tests(Albums): Update => Failure (bad request)``(title, coverLocation) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Albums.Update.Request(
                Id = 90000,
                Title = title,
                CoverLocation = coverLocation,
                PublishedAt = DateOnly())
            
            task {
                
                //Act
                let! httpMessage = client.PatchAsJsonAsync(url, body, JsonOptions().WithDateTimeOnly())
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("the title and cover location must have normal length (can break visual or takes a lot of memory)")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Albums): Update => Failure (bad request / conflict - publishedAt)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Albums.Update.Request(PublishedAt = DateOnly.MaxValue)
            
            task {
                
                //Act
                let! httpMessage = client.PatchAsJsonAsync(url, body, JsonOptions().WithDateTimeOnly())
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("the publication date cannot be greater than the current date and created date")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Albums): Update => Failure (not found)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Albums.Update.Request(
                Id = 95000,
                Title = "Test",
                CoverLocation = "Location",
                PublishedAt = DateOnly())
            
            task {
                
                //Act
                let! httpMessage = client.PatchAsJsonAsync(url, body, JsonOptions().WithDateTimeOnly())
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("the publication date cannot be greater than the current date and created date")
                    |> ignore
            }