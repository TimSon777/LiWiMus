namespace LiWiMus.Web.API.Tests.Playlists

open LiWiMus.Web.API.Shared
open FluentAssertions
open LiWiMus.Web.API.Tests
open Xunit

type ReadTests(factory: TestApplicationFactory) =
    let url = RouteConstants.Playlists.Read
    interface IClassFixture<TestApplicationFactory>
    
    [<Fact>]
    member this.``Tests(Playlists): Read => Success``() =
    
        // Arrange
        let client = factory.CreateClient()
    
        task {
    
            //Act
            let! httpMessage = client.GetAsync(url.Replace("{id:int}", "24"))
    
            //Assert
            httpMessage
                .Should()
                .BeSuccessful("playlist with id 1 must be in db (see seeder)")
            |> ignore
        }

    [<Fact>]
    member this.``Tests(Playlists): Read => Failure (not found)``() =
    
        // Arrange
        let client = factory.CreateClient()
    
        task {
    
            //Act
            let! httpMessage = client.GetAsync(url.Replace("{id:int}", "100"))
    
            //Assert
            httpMessage
                .Should()
                .HaveClientError("request to not existing entity must return 404")
            |> ignore
        }