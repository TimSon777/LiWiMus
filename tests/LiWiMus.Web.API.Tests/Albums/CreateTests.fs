module LiWiMus.Web.API.Tests.Albums

open System
open System.IO
open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests.WebApplicationFactory
open Xunit
open System.Net.Http.Json
open FluentAssertions
open Microsoft.AspNetCore.Http.Json
open LiWiMus.Web.API.Tests.JsonHelpers

type CreateTests(
    factory: TestApplicationFactory) =
    let url = Path.Combine("https://localhost:7061/api", RouteConstants.Albums.Create)
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
            PublishedAt = DateOnly(),
            ArtistIds = [|1|])
        
        //Act
        task {
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
    member this.``Tests(Albums): Create => BadRequest - Invalid title (long / small length)``(title, coverLocation) =
        
        // Arrange
        let client = factory.CreateClient()

        let body = Albums.Create.Request(
            Title = title,
            CoverLocation = coverLocation,
            PublishedAt = DateOnly(),
            ArtistIds = [|1|])
        
        //Act
        task {
            let! httpMessage = client.PostAsJsonAsync(url, body, JsonOptions().WithDateTimeOnly())
            
            //Assert
            httpMessage
                .Should()
                .HaveClientError("the title must has normal length (can break visual or takes a lot of memory)")
                |> ignore
        }
        
    [<Fact>]
    member this.``Tests(Albums): Create => BadRequest - Invalid publishedAt (more then current date)``() =
        
        // Arrange
        let client = factory.CreateClient()

        let body = Albums.Create.Request(
            Title = "Test",
            CoverLocation = "Location",
            PublishedAt = DateOnly.MaxValue,
            ArtistIds = [|1|])
        
        //Act
        task {
            let! httpMessage = client.PostAsJsonAsync(url, body, JsonOptions().WithDateTimeOnly())
            
            //Assert
            httpMessage
                .Should()
                .HaveClientError("the publication date cannot be greater than the current date")
                |> ignore
        }
    
    [<Fact>]
    [<InlineData>]
    member this.``Tests(Albums): Create => BadRequest - Invalid artistIds (empty list)``() =
        
        // Arrange
        let client = factory.CreateClient()

        let body = Albums.Create.Request(
            Title = "Test",
            CoverLocation = "Location",
            PublishedAt = DateOnly(),
            ArtistIds = [||])
        
        //Act
        task {
            let! httpMessage = client.PostAsJsonAsync(url, body, JsonOptions().WithDateTimeOnly())
            
            //Assert
            httpMessage
                .Should()
                .HaveClientError("the album must have artists")
                |> ignore
        }
        
    [<Fact>]
    [<InlineData>]
    member this.``Tests(Albums): Create => BadRequest - Invalid artistIds (one or more artists not exist)``() =
        
        // Arrange
        let client = factory.CreateClient()

        let body = Albums.Create.Request(
            Title = "Test",
            CoverLocation = "Location",
            PublishedAt = DateOnly(),
            ArtistIds = [|777|])
        
        //Act
        task {
            let! httpMessage = client.PostAsJsonAsync(url, body, JsonOptions().WithDateTimeOnly())
            
            //Assert
            httpMessage
                .Should()
                .HaveClientError("the album must have existed artists")
                |> ignore
        }    