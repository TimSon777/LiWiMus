namespace LiWiMus.Web.API.Tests.Playlists.Photo

open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions

type RemoveTests(factory: TestApplicationFactory) =
    let url = RouteConstants.Playlists.RemovePhoto
    interface IClassFixture<TestApplicationFactory>
    
    [<Fact>]
    member this.``Tests(Playlists): RemovePhoto => Success``() =
    
        // Arrange
        let client = factory.CreateClient()
    
        task {
    
            //Act
            let! httpMessage = client.PostAsync(url.Replace("{id:int}", "24"), null)
    
            //Assert
            httpMessage
                .Should()
                .BeSuccessful("playlist with id 24 must be in db (see seeder)")
            |> ignore
        }

    [<Fact>]
    member this.``Tests(Playlists): RemovePhoto => Failure (not found)``() =
    
        // Arrange
        let client = factory.CreateClient()
    
        task {
    
            //Act
            let! httpMessage = client.PostAsync(url.Replace("{id:int}", "100"), null)
    
            //Assert
            httpMessage
                .Should()
                .HaveClientError("request to not existing entity must return 404")
            |> ignore
        }
    