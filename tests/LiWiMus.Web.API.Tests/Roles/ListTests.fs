namespace LiWiMus.Web.API.Tests.Roles

open LiWiMus.Web.API.Shared
open FluentAssertions
open LiWiMus.Web.API.Tests
open Xunit
open LiWiMus.Web.API
open LiWiMus.Web.API.Tests.HttpResponseMessage
type ListTests(factory: TestApplicationFactory) =
    let url = RouteConstants.Roles.List
    interface IClassFixture<TestApplicationFactory>
    
    [<Fact>]
    member this.``Tests(Roles): List => Success``() =
    
        // Arrange
        let client = factory.CreateClient()
    
        task {
    
            //Act
            let! httpMessage = client.GetAsync(url)
            
            //Assert
            httpMessage
                .Should()
                .BeSuccessful("database must contains some roles") |> ignore
                
            let! list = httpMessage.ToArrayAsync<Plans.Dto>()
            
            list
                .Should()
                .NotBeEmpty("database must contains some roles")
            |> ignore
        }