namespace LiWiMus.Web.API.Tests.Users.Roles

open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions
open LiWiMus.Web.Shared.Extensions

type RemoveTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.Users.Roles.Remove
        interface IClassFixture<TestApplicationFactory>
    
        [<Fact>]
        member this.``Tests(Users -> Roles): Remove => Success``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Users.Roles.Remove.Request(
                UserId = 380000,
                RoleId = 380000)
            
            task {
                
                //Act
                let! httpMessage = client.DeleteAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .BeSuccessful("it should be possible to delete a role from a user")
                    |> ignore
            }
        [<Fact>]
        member this.``Tests(Users -> Roles): Add => Failure (unprocessable entity - role)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Users.Roles.Add.Request(
                UserId = 380004,
                RoleId = 385000)
            
            task {
                
                //Act
                let! httpMessage = client.DeleteAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("cannot delete a non-existent role from a user")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Users -> Roles): Add => Failure (unprocessable entity - user)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Users.Roles.Add.Request(
                UserId = 385000,
                RoleId = 380000)
            
            task {
                
                //Act
                let! httpMessage = client.DeleteAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("cannot delete a non-existent role from a user")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Users -> Roles): Add => Failure (unprocessable entity - user not in role)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Users.Roles.Add.Request(
                UserId = 380003,
                RoleId = 380000)
            
            task {
                
                //Act
                let! httpMessage = client.DeleteAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("cannot delete a role from a user if he doesn't have one")
                    |> ignore
            }
