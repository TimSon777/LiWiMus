namespace LiWiMus.Web.API.Tests.UserPlans

open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions

type ReadTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.UserPlans.Read
        interface IClassFixture<TestApplicationFactory>
    
        [<Theory>]
        [<InlineData(1)>]
        [<InlineData(2)>]
        member this.``Tests(UserPlans): Read => Success``(id: int) =
            
            // Arrange
            let client = factory.CreateClient()
            
            task {
                
                //Act
                let! httpMessage = client.GetAsync(url.Replace("{id:int}", id.ToString()))
                
                //Assert
                httpMessage
                    .Should()
                    .BeSuccessful($"user plan with id {id} must exists (see seeder)")
                    |> ignore
            }
        [<Theory>]
        [<InlineData(666777)>]
        [<InlineData(666778)>]
        [<InlineData(666779)>]
        member this.``Tests(UserPlans): Read => Failure (unprocessable entity)``(id: int) =
            
            // Arrange
            let client = factory.CreateClient()
            
            task {
                
                //Act
                let! httpMessage = client.GetAsync(url.Replace("{id:int}", id.ToString()))
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError($"user plan with id {id} doesn't exist (see seeder)")
                    |> ignore
            }