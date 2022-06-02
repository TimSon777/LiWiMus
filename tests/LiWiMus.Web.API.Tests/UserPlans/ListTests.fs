namespace LiWiMus.Web.API.Tests.UserPlans

open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions
open LiWiMus.Web.API.Tests.UserPlans.TestsHelpers

type ListTests(
        factory: TestApplicationFactory) =
        let baseUrl = RouteConstants.UserPlans.List
       
        
        interface IClassFixture<TestApplicationFactory>

        [<Theory>]
        [<InlineData(1, 2, true)>]
        [<InlineData(1, 1, true)>]
        [<InlineData(1, 2, false)>]
        [<InlineData(null, null, null)>]
        [<InlineData(1, null, null)>]
        [<InlineData(null, 2, null)>]
        [<InlineData(1, 2, null)>]
        member this.``Tests(User Plans): List => Success``(userId, planId, active) =
            
            // Arrange
            let client = factory.CreateClient()
            let url = AddQueries baseUrl userId planId (active |> string)
            
            task {
                
                //Act
                let! httpMessage = client.GetAsync(url)
                
                //Assert
                httpMessage
                    .Should()
                    .BeSuccessful("all params are valid")
                |> ignore
            }
       