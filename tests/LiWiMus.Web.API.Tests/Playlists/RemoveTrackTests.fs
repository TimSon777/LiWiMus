namespace LiWiMus.Web.API.Tests.Playlists

open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions
open System.Net.Http.Json
open LiWiMus.Web.Shared.Extensions

type RemoveTrackTests(factory: TestApplicationFactory) =
    let url = RouteConstants.Playlists.Tracks.Remove
    interface IClassFixture<TestApplicationFactory>
    
    [<Fact>]
    member this.``Tests(Playlists): RemoveTrack => Success``() =
    
        // Arrange
        let client = factory.CreateClient()
        let body = Playlists.Tracks.Remove.Request(TrackId = 1, PlaylistId = 1)
        
        task {
    
            //Act
            let! httpMessage = client.DeleteAsJsonAsync(url, body)
    
            //Assert
            httpMessage
                .Should()
                .BeSuccessful("track with id 1 is in playlist with id 1 (see seeder)")
            |> ignore
        }

    [<Theory>]
    [<InlineData(100)>]
    [<InlineData(999)>]
    member this.``Tests(Playlists): RemoveTrack => Failure (conflict / not found)``(trackId) =
    
        // Arrange
        let client = factory.CreateClient()
        let body = Playlists.Tracks.Remove.Request(TrackId = trackId, PlaylistId = 1)
        
        task {
    
            //Act
            let! httpMessage = client.DeleteAsJsonAsync(url, body)

            //Assert
            httpMessage
                .Should()
                .HaveClientError("track not exists or not in playlist")
            |> ignore
        }