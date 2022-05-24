namespace LiWiMus.Web.API.Tests.Plans.Permissions

open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions
open System.Net.Http.Json

type ReplaceAllTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.Plans.Permissions.ReplaceAll
        interface IClassFixture<TestApplicationFactory>
    
        [<Fact>]
        member this.``Tests(Plans -> Permissions): Add => Success``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Plans.Permissions.ReplaceAll.Request(
                PlanId = 180000,
                Permissions = [|1; 2; 3; 4|])
            
            task {
                
                //Act
                let! httpMessage = client.PutAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .BeSuccessful("plan and permissions must exists (see Validators)")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Plans -> Permissions): Add => Failure (unprocessable entity - plan)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Plans.Permissions.ReplaceAll.Request(
                PlanId = 185000,
                Permissions = [|1; 2; 3|])
            
            task {
                
                //Act
                let! httpMessage = client.PutAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("plan with id 185000 doesn't exists")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Plans -> Permissions): Add => Failure (unprocessable entity - permission)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Plans.Permissions.ReplaceAll.Request(
                PlanId = 180000,
                Permissions = [|1; 2; 3; 400|])
            
            task {
                
                //Act
                let! httpMessage = client.PutAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("permission with id 400 doesn't exists")
                    |> ignore
            }
      