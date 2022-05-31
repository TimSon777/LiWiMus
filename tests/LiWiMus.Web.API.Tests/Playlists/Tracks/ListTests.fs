namespace LiWiMus.Web.API.Tests.Playlists.Tracks

open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions

type ListTests(
    factory: TestApplicationFactory) =
    let baseUrl = RouteConstants.Playlists.Tracks.List
    interface IClassFixture<TestApplicationFactory>

    [<Theory>]
    [<InlineData(24, 1, 2)>]
    [<InlineData(24, 2, 5)>]
    member this.``Tests(Playlists -> Tracks): List => Success``(playlistId, page, itemsPerPage) =
        
        // Arrange
        let client = factory.CreateClient()
    
        let url = TestHelpers.GenerateUrl(baseUrl, playlistId, "{playlistId:int}", page, itemsPerPage)
        
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
    [<InlineData(24, -1, 2)>]
    [<InlineData(24, 2, -5)>]
    [<InlineData(24, 0, 5)>]
    [<InlineData(24, 2, 0)>]
    member this.``Tests(Playlists -> Tracks): List => Failure (bad request)``(playlistId, page, itemsPerPage) =
        
        // Arrange
        let client = factory.CreateClient()
    
        let url = TestHelpers.GenerateUrl(baseUrl, playlistId, "{playlistId:int}", page, itemsPerPage)
                    
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
    member this.``Tests(Playlists -> Tracks): List => Failure (unprocessable entity)``() =
        
        // Arrange
        let client = factory.CreateClient()
    
        let url = TestHelpers.GenerateUrl(baseUrl, 9999999, "{playlistId:int}", 1, 1)
                    
        task {
            
            //Act
            let! httpMessage = client.GetAsync(url)
            
            //Assert
            httpMessage
                .Should()
                .HaveClientError("playlist with such id doesn't exist")
                |> ignore
        }
            