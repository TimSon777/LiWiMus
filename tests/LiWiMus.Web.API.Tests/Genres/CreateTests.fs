namespace LiWiMus.Web.API.Tests.Genres

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
        let url = RouteConstants.Genres.Create
        interface IClassFixture<TestApplicationFactory>
    
        [<Theory>]
        [<InlineData("GenreTest")>]
        [<InlineData("Al")>]
        [<InlineData("Longgggggggggggggggggggggggggggggg")>]
        [<InlineData("123")>]
        member this.``Tests(Genres): Create => Success``(name) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Genres.Create.Request(Name = name)
            
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
        [<InlineData("X")>]
        [<InlineData("VeryLongggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg")>]
        member this.``Tests(Genres): Create => Failure (bad request)``(name) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Genres.Create.Request(Name = name)
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body, JsonOptions().WithDateTimeOnly())
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("the name must has normal length (can break visual or takes a lot of memory)")
                    |> ignore
            }
        
        [<Fact>]
        member this.``Tests(Genres): Create => Failure (conflict - already exists)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Genres.Create.Request(Name = "string")
            
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
                    .HaveClientError("the genre with same name already exists")
                    |> ignore
            }
