namespace LiWiMus.Web.API.Tests.Plans

open System
open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open System.Net.Http.Json
open FluentAssertions
open Microsoft.AspNetCore.Http.Json
open LiWiMus.Web.API.Tests.JsonHelpers

type CreateTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.Plans.Create
        interface IClassFixture<TestApplicationFactory>
    
        [<Theory>]
        [<InlineData("TestName1", "Description", 100)>]
        [<InlineData("TestName2", "Description", 0.1)>]
        [<InlineData("TestName3", "Description", 999)>]
        [<InlineData("TestName4_Longgggggggggggggggggg", "Description", 999)>]
        member this.``Tests(Plans): Create => Success``(name, description, pricePerMonth) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Plans.Create.Request(
                Name = name,
                Description = description,
                PricePerMonth = pricePerMonth)
            
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
        [<InlineData("TestName10", "Description", 100)>]
        [<InlineData("TestName11", "Description", 0.1)>]
        [<InlineData("TestName12", "Description", 999)>]
        [<InlineData("TestName13", "Description", 999)>]
        member this.``Tests(Plans): Create => Failure (name)``(name, description, pricePerMonth) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Plans.Create.Request(
                Name = name,
                Description = description,
                PricePerMonth = pricePerMonth)
            
            task {
                
                //Act
                let! httpMessageSuccess = client.PostAsJsonAsync(url, body)
                let! httpMessageFailure = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessageSuccess
                    .Should()
                    .BeSuccessful("all params must be valid (see Validator)")
                    |> ignore
                    
                httpMessageFailure
                    .Should()
                    .HaveClientError("plan with same name already exists")
                    |> ignore
            }
            
            
        [<Theory>]
        [<InlineData("NameLonggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg", "Description", 100)>]
        [<InlineData(null, "Description", 0.1)>]
        [<InlineData("<Tags>", "Description", 999)>]
        [<InlineData("A<Tags>A", "Description", 999)>]
        [<InlineData("A<Tags>", "Description", 999)>]
        [<InlineData("<Tags>A", "Description", 999)>]
        [<InlineData("TestName13", "<Tags>", 999)>]
        [<InlineData("TestName13", "Description", 0)>]
        [<InlineData("TestName13", "Description", -100)>]
        [<InlineData("TestName13", null, 10)>]
        member this.``Tests(Plans): Create => Failure (bad request)``(name, description, pricePerMonth) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Plans.Create.Request(
                Name = name,
                Description = description,
                PricePerMonth = pricePerMonth)
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("plan with same name already exists")
                    |> ignore
            }