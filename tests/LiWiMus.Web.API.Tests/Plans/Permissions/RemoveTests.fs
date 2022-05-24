namespace LiWiMus.Web.API.Tests.Plans.Permissions

open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions
open LiWiMus.Web.Shared.Extensions

type DeleteTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.Plans.Permissions.Remove
        interface IClassFixture<TestApplicationFactory>
    
        [<Fact>]
        member this.``Tests(Plans -> Permissions): Remove => Success``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Plans.Permissions.Remove.Request(
                PlanId = 180000,
                PermissionId = 4)
            
            task {
                
                //Act
                let! httpMessage = client.DeleteAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .BeSuccessful("plan with id 180000 must has permission with id 11 (see seeder)")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Plans -> Permissions): Remove => Failure (unprocessable entity - plan)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Albums.Owners.Remove.Request(
                Id = 185000,
                ArtistId = 5)
            
            task {
                
                //Act
                let! httpMessage = client.DeleteAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("plan with id 185000 not exists")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Plans -> Permissions): Remove => Failure (unprocessable entity - permission)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Plans.Permissions.Remove.Request(
                PlanId = 180000,
                PermissionId = 185000)
            
            task {
                
                //Act
                let! httpMessage = client.DeleteAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("permission with id 185000 not exists")
                    |> ignore
            }
        