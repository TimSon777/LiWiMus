namespace LiWiMus.Web.API.Tests.Roles

open LiWiMus.Web.API.Shared
open FluentAssertions
open LiWiMus.Web.API.Tests
open Xunit

type ReadTests(factory: TestApplicationFactory) =
    let url = RouteConstants.Roles.Read
    interface IClassFixture<TestApplicationFactory>
    
    [<Fact>]
    member this.``Tests(Roles): Read => Success``() =
    
        // Arrange
        let client = factory.CreateClient()
        let id = 1
        
        task {
    
            //Act
            let! httpMessage = client.GetAsync(url.Replace("{id:int}", id.ToString()))
    
            //Assert
            httpMessage
                .Should()
                .BeSuccessful($"role with id {id} must be in db (see seeder)")
            |> ignore
        }

    [<Fact>]
    member this.``Tests(Roles): Read => Failure (not found)``() =
    
        // Arrange
        let client = factory.CreateClient()
        let id = 13223
        
        task {
    
            //Act
            let! httpMessage = client.GetAsync(url.Replace("{id:int}", id.ToString()))
    
            //Assert
            httpMessage
                .Should()
                .HaveClientError("request to not existing entity must return 404")
            |> ignore
        }