namespace LiWiMus.Web.API.Tests.Users

open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open System.Net.Http.Json
open FluentAssertions
open Microsoft.AspNetCore.Http.Json
open LiWiMus.Web.API.Tests.JsonOptionsExtensions

type CreateTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.Users.Create
        interface IClassFixture<TestApplicationFactory>
    
        [<Theory>]
        [<InlineData("TestUserName1", "testEmail1@mail.test")>]
        [<InlineData("TestUserName2", "testEmail2@mail.test")>]
        [<InlineData("TestUserName3", "testEmail3@mail.test")>]
        member this.``Tests(Users): Create => Success``(userName, email) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Users.Create.Request(
                UserName = userName,
                Email = email,
                Password = "Password"
            )
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .BeSuccessful("all params must be valid (see Validator)")
                    |> ignore
            }
            
        [<Theory>]
        [<InlineData("TestUserName1", "testEmail1@mail.test", "")>]
        [<InlineData("<Tags>", "testEmail3@mail.test", "Password")>]
        [<InlineData("A<Tags>", "testEmail3@mail.test", "Password")>]
        [<InlineData("<Tags>A", "testEmail3@mail.test", "Password")>]
        [<InlineData("A<Tags>A", "testEmail3@mail.test", "Password")>]
        [<InlineData("TestUserName2", "notEmail", "Password")>]
        [<InlineData("TestUserName2", "", "Password")>]
        [<InlineData("TestUserName2", null, "Password")>]
        [<InlineData(null, "testEmail4@mail.test", "Password")>]
        [<InlineData("TestUserName24", "testEmail4@mail.test", null)>]
        member this.``Tests(Users): Create => Failure (bad request)``(userName, email, password) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Users.Create.Request(
                UserName = userName,
                Email = email,
                Password = password
            )
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body, JsonOptions().WithDateTimeOnly())
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("must be bad request - params invalid (maybe requirements were changed?)")
                |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Users): Create => Failure (conflict)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Users.Create.Request(
                UserName = "UserName_Conflict",
                Email = "testEmail100@mail.test",
                Password = "Password"
            )
            
            task {
                
                //Act
                let! httpMessageSuccess = client.PostAsJsonAsync(url, body)
                let! httpMessageFailure = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessageSuccess
                    .Should()
                    .BeSuccessful("the publication date cannot be greater than the current date")
                |> ignore
                
                httpMessageFailure
                    .Should()
                    .HaveClientError("user with same name already exists")
                |> ignore
            }
