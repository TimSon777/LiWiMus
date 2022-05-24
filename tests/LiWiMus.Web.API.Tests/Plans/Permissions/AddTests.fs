namespace LiWiMus.Web.API.Tests.Plans.Permissions

open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions
open System.Net.Http.Json

type AddTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.Plans.Permissions.Add
        interface IClassFixture<TestApplicationFactory>
    
        [<Theory>]
        [<InlineData(1)>]
        [<InlineData(2)>]
        member this.``Tests(Plans -> Permissions): Add => Success``(permissionId) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Plans.Permissions.Add.Request(
                PlanId = 180000,
                PermissionId = permissionId)
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
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
    
            let body = Plans.Permissions.Add.Request(
                PlanId = 185000,
                PermissionId = 1)
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
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
    
            let body = Plans.Permissions.Add.Request(
                PlanId = 180000,
                PermissionId = 185000)
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("permission with id 185000 doesn't exists")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Plans -> Permissions): Add => Failure (conflict)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let planId = 180000
            
            let body = Plans.Permissions.Add.Request(
                PlanId = planId,
                PermissionId = 3)
            
            task {
                
                //Act
                let! httpMessageSuccess = client.PostAsJsonAsync(url, body)
                let! httpMessageFailure = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessageSuccess
                    .Should()
                    .BeSuccessful("plan and permissions must exists (see Validators)")
                |> ignore
                
                httpMessageFailure
                    .Should()
                    .HaveClientError($"permission with id 1 already added to plan with id {planId}")
                |> ignore
            }
        