namespace LiWiMus.Web.API.Tests.Albums.Owners

open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions
open LiWiMus.Web.Shared.Extensions

type DeleteTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.Albums.Artists.Remove
        interface IClassFixture<TestApplicationFactory>
    
        [<Theory>]
        [<InlineData(90000)>]
        [<InlineData(90001)>]
        member this.``Tests(Albums -> Owners): Delete => Success``(artistId) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Albums.Owners.Remove.Request(
                Id = 90000,
                ArtistId = artistId)
            
            task {
                
                //Act
                let! httpMessage = client.DeleteAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .BeSuccessful("artist must exists (see Validator)")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Albums): Update => Failure (unprocessable entity - artist)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Albums.Owners.Remove.Request(
                Id = 90000,
                ArtistId = 95000)
            
            task {
                
                //Act
                let! httpMessage = client.DeleteAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("can't delete not existed artist (maybe you have problem with seeder?)")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Albums): Update => Failure (unprocessable entity - album)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Albums.Owners.Remove.Request(
                Id = 95000,
                ArtistId = 90000)
            
            task {
                
                //Act
                let! httpMessage = client.DeleteAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("can't found not existed album (maybe you have problem with seeder?)")
                    |> ignore
            }
        