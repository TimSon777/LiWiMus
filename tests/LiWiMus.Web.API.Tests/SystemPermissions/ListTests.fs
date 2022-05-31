namespace LiWiMus.Web.API.Tests.SystemPermissions

open LiWiMus.Web.API.Shared
open FluentAssertions
open LiWiMus.Web.API.Tests
open Xunit
open LiWiMus.Web.API
open LiWiMus.Web.API.Tests.HttpResponseMessage
type ListTests(factory: TestApplicationFactory) =
    let url = RouteConstants.SystemPermissions.List
    interface IClassFixture<TestApplicationFactory>
    
    [<Fact>]
    member this.``Tests(SystemPermissions): List => Success``() =
    
        // Arrange
        let client = factory.CreateClient()
    
        task {
    
            //Act
            let! httpMessage = client.GetAsync(url)
            
            //Assert
            httpMessage
                .Should()
                .BeSuccessful("database must contains some system permissions") |> ignore
                
            let! list = httpMessage.ToArrayAsync<Plans.Dto>()
            
            list
                .Should()
                .NotBeEmpty("response must contains some system permissions")
            |> ignore
        }