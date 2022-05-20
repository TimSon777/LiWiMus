namespace LiWiMus.Web.API.Tests.Playlists

open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions

type DeleteTests(factory: TestApplicationFactory) =
    let url = RouteConstants.Playlists.Delete
    interface IClassFixture<TestApplicationFactory>
    
    [<Fact>]
    member this.``Tests(Playlists): Delete => Success``() =
    
        // Arrange
        let client = factory.CreateClient()
    
        task {
    
            //Act
            let! httpMessage = client.DeleteAsync(url.Replace("{id:int}", "1"))
    
            //Assert
            httpMessage
                .Should()
                .BeSuccessful("playlist with id 1 must be in db (see seeder)")
            |> ignore
        }

    [<Fact>]
    member this.``Tests(Playlists): Delete => Failure (not found)``() =
    
        // Arrange
        let client = factory.CreateClient()
    
        task {
    
            //Act
            let! httpMessage = client.DeleteAsync(url.Replace("{id:int}", "100"))
    
            //Assert
            httpMessage
                .Should()
                .HaveClientError("request to not existing entity must return 404")
            |> ignore
        }
    