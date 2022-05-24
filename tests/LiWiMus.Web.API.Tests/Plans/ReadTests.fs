namespace LiWiMus.Web.API.Tests.Plans

open LiWiMus.Web.API.Shared
open FluentAssertions
open LiWiMus.Web.API.Tests
open Xunit

type ReadTests(factory: TestApplicationFactory) =
    let url = RouteConstants.Plans.Read
    interface IClassFixture<TestApplicationFactory>
    
    [<Fact>]
    member this.``Tests(Plans): Read => Success``() =
    
        // Arrange
        let client = factory.CreateClient()
    
        task {
    
            //Act
            let! httpMessage = client.GetAsync(url.Replace("{id:int}", "180000"))
    
            //Assert
            httpMessage
                .Should()
                .BeSuccessful("plan with id 1 must be in db (see seeder)")
            |> ignore
        }

    [<Fact>]
    member this.``Tests(Plans): Read => Failure (not found)``() =
    
        // Arrange
        let client = factory.CreateClient()
    
        task {
    
            //Act
            let! httpMessage = client.GetAsync(url.Replace("{id:int}", "185000"))
    
            //Assert
            httpMessage
                .Should()
                .HaveClientError("request to not existing entity must return 404")
            |> ignore
        }