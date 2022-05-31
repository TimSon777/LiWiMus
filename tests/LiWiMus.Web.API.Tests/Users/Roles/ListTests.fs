namespace LiWiMus.Web.API.Tests.Users.Roles

open LiWiMus.Web.API.Shared
open FluentAssertions
open LiWiMus.Web.API.Tests
open Xunit
open LiWiMus.Web.API
open LiWiMus.Web.API.Tests.HttpResponseMessage

type ListTests(factory: TestApplicationFactory) =
    let url = RouteConstants.Users.Roles.List
    interface IClassFixture<TestApplicationFactory>
    
    [<Theory>]
    [<InlineData(380000)>]
    [<InlineData(380001)>]
    [<InlineData(380005)>]
    member this.``Tests(Users -> Roles): List => Success``(userId) =
    
        // Arrange
        let client = factory.CreateClient()
        
        task {
    
            //Act
            let! httpMessage = client.GetAsync(url.Replace("{userId:int}", userId.ToString()))
            
            //Assert
            httpMessage
                .Should()
                .BeSuccessful("database must contains some users' plans") |> ignore
                
            let! list = httpMessage.ToArrayAsync<Roles.Dto>()
            
            list
                .Should()
                .NotBeEmpty("user must has some roles")
            |> ignore
        }
        
    [<Fact>]
    member this.``Tests(Users -> Roles): List => Failure (unprocessable entity)``() =
    
        // Arrange
        let client = factory.CreateClient()
        let userId = 385000
        
        task {
    
            //Act
            let! httpMessage = client.GetAsync(url.Replace("{userId:int}", userId.ToString()))
            
            //Assert
            httpMessage
                .Should()
                .HaveClientError("database must contains some users' plans") |> ignore
        }