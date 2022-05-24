namespace LiWiMus.Web.API.Tests.Plans

open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions

type DeleteTests(factory: TestApplicationFactory) =
    let url = RouteConstants.Plans.Delete
    interface IClassFixture<TestApplicationFactory>
    
    [<Fact>]
    member this.``Tests(Playlists): Delete => Success``() =
    
        // Arrange
        let client = factory.CreateClient()
    
        task {
    
            //Act
            let! httpMessage = client.DeleteAsync(url.Replace("{id:int}", "180000"))
    
            //Assert
            httpMessage
                .Should()
                .BeSuccessful("playlist with id 180000 must be in db (see seeder)")
            |> ignore
        }

    [<Fact>]
    member this.``Tests(Playlists): Delete => Failure (not found)``() =
    
        // Arrange
        let client = factory.CreateClient()
    
        task {
    
            //Act
            let! httpMessage = client.DeleteAsync(url.Replace("{id:int}", "185000"))
    
            //Assert
            httpMessage
                .Should()
                .HaveClientError("request to not existing entity must return 404")
            |> ignore
        }
        
    [<Theory>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    member this.``Tests(Playlists): Delete => Failure (not access)``(id) =
    
        // Arrange
        let client = factory.CreateClient()
    
        task {
    
            //Act
            let! httpMessage = client.DeleteAsync(url.Replace("{id:int}", id.ToString()))
    
            //Assert
            httpMessage
                .Should()
                .HaveClientError($"can't delete default plan: id {id}")
            |> ignore
        }
    