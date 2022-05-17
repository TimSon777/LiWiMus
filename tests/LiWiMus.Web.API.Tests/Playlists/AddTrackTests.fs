namespace LiWiMus.Web.API.Tests.Playlists

open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests.WebApplicationFactory
open Xunit
open FluentAssertions
open System.Net.Http.Json

type AddTrackTests(factory: TestApplicationFactory) =
    let url = RouteConstants.Playlists.Tracks.Add
    interface IClassFixture<TestApplicationFactory>
    
    [<Fact>]
    member this.``Tests(Playlists): AddTrack => Success``() =
    
        // Arrange
        let client = factory.CreateClient()
        let body = Playlists.Tracks.Add.Request(TrackId = 100, PlaylistId = 1)
        
        task {
    
            //Act
            let! httpMessage = client.PostAsJsonAsync(url, body)
    
            //Assert
            httpMessage
                .Should()
                .BeSuccessful("playlist with id 1 and track with id 100 must be in db (see seeder)")
            |> ignore
        }

    [<Fact>]
    member this.``Tests(Playlists): AddTrack => Failure (conflict)``() =
    
        // Arrange
        let client = factory.CreateClient()
        let body = Playlists.Tracks.Add.Request(TrackId = 101, PlaylistId = 1)
        
        task {
    
            //Act
            let! firstHttpMessage = client.PostAsJsonAsync(url, body)
            let! secondHttpMessage = client.PostAsJsonAsync(url, body)
            
            //Assert
            firstHttpMessage
                .Should()
                .BeSuccessful("this track yet not added")
            |> ignore
            
            secondHttpMessage
                .Should()
                .HaveClientError("this track already exists")
            |> ignore
        }
    