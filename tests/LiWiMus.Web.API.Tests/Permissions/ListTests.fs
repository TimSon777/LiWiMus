namespace LiWiMus.Web.API.Tests.Permissions

open LiWiMus.Web.API.Shared
open FluentAssertions
open LiWiMus.Web.API.Tests
open Xunit
open LiWiMus.Web.API
open LiWiMus.Web.API.Tests.HttpResponseMessage

type ListTests(factory: TestApplicationFactory) =
    let url = RouteConstants.Permissions.List
    interface IClassFixture<TestApplicationFactory>
    
    [<Fact>]
    member this.``Tests(Permissions): List => Success``() =
    
        // Arrange
        let client = factory.CreateClient()
    
        task {
    
            //Act
            let! httpMessage = client.GetAsync(url)
            
            //Assert
            httpMessage
                .Should()
                .BeSuccessful("database must contains some permissions") |> ignore
                
            let! list = httpMessage.ToArrayAsync<SystemPermissions.Dto>()
            
            list
                .Should()
                .NotBeEmpty("response must contains some permissions")
            |> ignore
        }