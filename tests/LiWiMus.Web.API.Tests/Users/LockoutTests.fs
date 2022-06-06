namespace LiWiMus.Web.API.Tests.Users

open System
open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open System.Net.Http.Json
open FluentAssertions

type LockoutTests(
    factory: TestApplicationFactory) =
    let url = RouteConstants.Users.LockOut
    interface IClassFixture<TestApplicationFactory>
    
    [<Fact>]
    member this.``Tests(Users): Lockout => Success``() =
        
        // Arrange
        let client = factory.CreateClient()
        let body = Users.LockOut.Request(End = DateTime.MaxValue)
        let userId = 380000
        
        task {
            
            //Act
            let! httpMessage = client.PostAsJsonAsync(url.Replace("{id:int}", userId.ToString()), body)
            
            //Assert
            httpMessage
                .Should()
                .BeSuccessful("all params must be valid (see Validator)")
                |> ignore
        }
        
    [<Fact>]
    member this.``Tests(Users): Lockout => Failure (unprocessable entity)``() =
        
        // Arrange
        let client = factory.CreateClient()
        let body = Users.LockOut.Request(End = DateTime.MaxValue)
        let userId = 380005
        
        task {
            
            //Act
            let! httpMessage = client.PostAsJsonAsync(url.Replace("{id:int}", userId.ToString()), body)
            
            //Assert
            httpMessage
                .Should()
                .HaveClientError("can not ban the admin")
                |> ignore
        }