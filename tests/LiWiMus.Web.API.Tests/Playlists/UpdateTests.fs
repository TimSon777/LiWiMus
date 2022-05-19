namespace LiWiMus.Web.API.Tests.Playlists

open LiWiMus.Web.API.Shared
open Xunit
open FluentAssertions
open LiWiMus.Web.API.Tests.WebApplicationFactory
open LiWiMus.Web.API
open LiWiMus.Web.Shared.Extensions

type UpdateTests(factory: TestApplicationFactory) =
    let url = RouteConstants.Playlists.Update
    interface IClassFixture<TestApplicationFactory>

    [<Theory>]
    [<InlineData(1, "NewTestName", true)>]
    [<InlineData(1, "NewTestName1", true)>]
    [<InlineData(1, "NewTestName2", true)>]
    [<InlineData(1, "NewTestName3", false)>]
    member this.``Tests(Playlists): Update => Success``(id, name, isPublic) =

        // Arrange
        let client = factory.CreateClient()

        let body =
            Playlists.Update.Request(
                Id = id,
                Name = name,
                IsPublic = isPublic
            )

        task {

            //Act
            let! httpMessage = client.PatchAsJsonAsync(url, body)

            //Assert
            httpMessage
                .Should()
                .BeSuccessful("all params must be valid (see Validator)")
            |> ignore
        }
        
        
    [<Fact>]
    member this.``Tests(Playlists): Update => Success (send only id)``() =

        // Arrange
        let client = factory.CreateClient()

        let body =
            Playlists.Update.Request(Id = 1)

        task {

            //Act
            let! httpMessage = client.PatchAsJsonAsync(url, body)

            //Assert
            httpMessage
                .Should()
                .BeSuccessful("you can send half-empty request (patch method)")
            |> ignore
        }
        
        
    [<Theory>]
    [<InlineData("VeryLongggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg")>]
    member this.``Tests(Playlists): Update => Failure (bad request)``(name) =
        
        // Arrange
        let client = factory.CreateClient()
        let body = Playlists.Update.Request(Id = 1, Name = name)
        
        task {
            
            //Act
            let! httpMessage = client.PatchAsJsonAsync(url, body)
            
            //Assert
            httpMessage
                .Should()
                .HaveClientError("the name must has normal length (can break visual or takes a lot of memory)")
                |> ignore
        }
        
    
    [<Fact>]
    member this.``Tests(Playlists): Update => Failure (not found)``() =
        
        // Arrange
        let client = factory.CreateClient()
        let body = Playlists.Update.Request(Id = 770)
        
        task {
            
            //Act
            let! httpMessage = client.PatchAsJsonAsync(url, body)
            
            //Assert
            httpMessage
                .Should()
                .HaveClientError("request to not existing entity must return 404")
                |> ignore
        }