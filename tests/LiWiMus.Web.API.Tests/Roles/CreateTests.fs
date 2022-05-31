namespace LiWiMus.Web.API.Tests.Roles

open LiWiMus.Core.Roles
open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open LiWiMus.Web.API.Tests.Roles.Generators
open Xunit
open System.Net.Http.Json
open FluentAssertions

type CreateTests(
    factory: TestApplicationFactory) =
    let url = RouteConstants.Roles.Create
    interface IClassFixture<TestApplicationFactory>
    
    [<Theory>]
    [<InlineData("TestRole1", "Description")>]
    [<InlineData("A", "L")>]
    member this.``Tests(Roles): Create => Success``(name, description) =
        
        // Arrange
        let client = factory.CreateClient()
    
        let body = Roles.Create.Request(
            Name = name,
            Description = description)
        
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
    [<ClassData(typeof<CreateTestsGeneratorFailure>)>]
    member this.``Tests(Roles): Create => Failure (bad request)``(name, description) =
        
        // Arrange
        let client = factory.CreateClient()
    
        let body = Roles.Create.Request(
            Name = name,
            Description = description)
        
        task {
            
            //Act
            let! httpMessage = client.PostAsJsonAsync(url, body)
            
            //Assert
            httpMessage
                .Should()
                .HaveClientError("cannot add a role with invalid params")
            |> ignore
        }
        
    [<Fact>]
    member this.``Tests(Roles): Create => Failure (conflict)``() =
        
        // Arrange
        let client = factory.CreateClient()
    
        let body = Roles.Create.Request(
            Name = DefaultRoles.Admin.Name,
            Description = "Description")
        
        task {
            
            //Act
            let! httpMessage = client.PostAsJsonAsync(url, body)
            
            //Assert
            httpMessage
                .Should()
                .HaveClientError("cannot create a role if the name is occupied")
                |> ignore
        }
