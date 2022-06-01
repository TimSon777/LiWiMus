namespace LiWiMus.Web.API.Tests.Roles

open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions

type DeleteTests(factory: TestApplicationFactory) =
    let url = RouteConstants.Roles.Delete
    interface IClassFixture<TestApplicationFactory>
    
    [<Fact>]
    member this.``Tests(Roles): Delete => Success``() =
    
        // Arrange
        let client = factory.CreateClient()
        let id = 990000
        
        task {
    
            //Act
            let! httpMessage = client.DeleteAsync(url.Replace("{id:int}", id.ToString()))
    
            //Assert
            httpMessage
                .Should()
                .BeSuccessful($"role with id {id} must be in db (see seeder)")
            |> ignore
        }

    [<Fact>]
    member this.``Tests(Roles): Delete => Failure (not found)``() =
    
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
    