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

type CreateTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.Albums.Create
        interface IClassFixture<TestApplicationFactory>
    
        [<Theory>]
        [<InlineData("AlbumTest", "Location")>]
        [<InlineData("Al", "L")>]
        [<InlineData("Longgggggggggggggggggggggggggggggg", "Longgggggggggggggggggggggggggggggg")>]
        [<InlineData("123", "321")>]
        member this.``Tests(Albums): Create => Success``(title, coverLocation) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Albums.Create.Request(
                Title = title,
                CoverLocation = coverLocation,
                PublishedAt = DateOnly())
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body, JsonOptions().WithDateTimeOnly())
                
                //Assert
                httpMessage
                    .Should()
                    .BeSuccessful("all params must be valid (see Validator)")
                    |> ignore
            }
            
        [<Theory>]
        [<InlineData("X", "X")>]
        [<InlineData("VeryLonggggggggggggggggggggggggggggggggggggggggggggggg", "Longgggggggggggggggggggggggggggggg")>]
        member this.``Tests(Albums): Create => Failure (bad request)``(title, coverLocation) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Albums.Create.Request(
                Title = title,
                CoverLocation = coverLocation,
                PublishedAt = DateOnly())
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body, JsonOptions().WithDateTimeOnly())
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("the title must has normal length (can break visual or takes a lot of memory)")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Albums): Create => Failure (bad request - publishedAt)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Albums.Create.Request(
                Title = "Test",
                CoverLocation = "Location",
                PublishedAt = DateOnly.MaxValue)
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body, JsonOptions().WithDateTimeOnly())
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("the publication date cannot be greater than the current date")
                    |> ignore
            }
