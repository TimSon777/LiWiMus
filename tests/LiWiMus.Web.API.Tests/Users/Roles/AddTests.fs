namespace LiWiMus.Web.API.Tests.Users.Roles

open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions
open System.Net.Http.Json

type AddTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.Users.Roles.Add
        interface IClassFixture<TestApplicationFactory>
    
        [<Fact>]
        member this.``Tests(Users -> Roles): Add => Success``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Users.Roles.Add.Request(
                UserId = 380001,
                RoleId = 380000)
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .BeSuccessful("it should be possible to add a role to a user if all the data is valid")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Users -> Roles): Add => Failure (unprocessable entity - role)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Users.Roles.Add.Request(
                UserId = 380002,
                RoleId = 385000)
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("cannot add a non-existent role to a user")
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
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("cannot add a role to a non-existent user")
                    |> ignore
            }
        
        [<Fact>]
        member this.``Tests(Users -> Roles): Add => Failure (conflict)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Users.Roles.Add.Request(
                UserId = 380000,
                RoleId = 380000)
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("cannot add the same role to a user")
                    |> ignore
            }