namespace LiWiMus.Web.API.Tests.Albums.Owners

open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions
open System.Net.Http.Json

type AddTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.Albums.Artists.Add
        interface IClassFixture<TestApplicationFactory>
    
        [<Fact>]
        member this.``Tests(Albums -> Owners): Add => Success``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Albums.Owners.Add.Request(
                Id = 90000,
                ArtistId = 90002)
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .BeSuccessful("artist must exists (see Validator)")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Albums -> Owners): Add => Failure (unprocessable entity - artist)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Albums.Owners.Remove.Request(
                Id = 90000,
                ArtistId = 95000)
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("can't delete not existed artist (maybe you have problem with seeder?)")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Albums -> Owners): Add => Failure (unprocessable entity - album)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Albums.Owners.Remove.Request(
                Id = 95000,
                ArtistId = 90000)
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("can't found not existed album (maybe you have problem with seeder?)")
                    |> ignore
            }
        