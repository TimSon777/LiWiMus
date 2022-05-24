namespace LiWiMus.Web.API.Tests.Albums.Owners

open System
open System.Collections.Generic
open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Microsoft.AspNetCore.WebUtilities
open Xunit
open FluentAssertions
open LiWiMus.Web.Shared.Extensions

type ListTests(
        factory: TestApplicationFactory) =
        let baseUrl = RouteConstants.Albums.Artists.List
        interface IClassFixture<TestApplicationFactory>
    
        member this.GenerateUrl(albumId, page:int, itemsPerPage:int) =
            let queries = [
                KeyValuePair<string, string>("page", page.ToString())
                KeyValuePair<string, string>("itemsPerPage", itemsPerPage.ToString())
            ]
            
            QueryHelpers.AddQueryString(baseUrl.Replace("{albumId:int}", albumId.ToString()), queries)

        [<Theory>]
        [<InlineData(90000, 1, 2)>]
        [<InlineData(90000, 2, 5)>]
        member this.``Tests(Albums -> Owners): List => Success``(albumId, page, itemsPerPage) =
            // Arrange
            let client = factory.CreateClient()
    
            let url = this.GenerateUrl(albumId, page, itemsPerPage)
            
            task {
                
                //Act
                let! httpMessage = client.GetAsync(url)
                
                //Assert
                httpMessage
                    .Should()
                    .BeSuccessful("all params must be valid")
                    |> ignore
            }
            
        [<Theory>]
        [<InlineData(90000, -1, 2)>]
        [<InlineData(90000, 2, -5)>]
        [<InlineData(90000, 0, 5)>]
        [<InlineData(90000, 2, 0)>]
        member this.``Tests(Albums -> Owners): List => Failure (bad request)``(albumId, page, itemsPerPage) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let url = this.GenerateUrl(albumId, page, itemsPerPage)
                        
            task {
                
                //Act
                let! httpMessage = client.GetAsync(url)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("endpoint must return bad request, if request params invalid")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Albums -> Owners): List => Failure (unprocessable entity)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let url = this.GenerateUrl(95000, 1, 1)
                        
            task {
                
                //Act
                let! httpMessage = client.GetAsync(url)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("album with such id id doesn't exist")
                    |> ignore
            }
            