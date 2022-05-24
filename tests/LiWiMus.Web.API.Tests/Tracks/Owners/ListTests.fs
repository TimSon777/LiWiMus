namespace LiWiMus.Web.API.Tests.Tracks.Owners

open System
open System.Collections.Generic
open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open LiWiMus.Web.API.Tests.HttpResponseMessage
open Xunit
open FluentAssertions
open LiWiMus.Web.Shared.Extensions

type ListTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.Tracks.Owners.List
        interface IClassFixture<TestApplicationFactory>

        [<Theory>]
        [<InlineData(220000)>]
        member this.``Tests(Tracks -> Owners): List => Success``(trackId) =
            // Arrange
            let client = factory.CreateClient()
            
            task {
                
                //Act
                let! httpMessage = client.GetAsync(url.Replace("{id:int}", trackId.ToString()))
                
                //Assert
                httpMessage
                    .Should()
                    .BeSuccessful($"track with id {trackId} must exists")
                |> ignore
                
                let! artists = httpMessage.ToArrayAsync<Artists.Dto>()
                
                artists
                    .Should()
                    .NotBeEmpty("track must has artists")
                |> ignore
            }
        
        [<Fact>]
        member this.``Tests(Tracks -> Owners): List => Failure (unprocessable entity)``() =
            // Arrange
            let client = factory.CreateClient()
            let trackId = 225000
            
            task {
                
                //Act
                let! httpMessage = client.GetAsync(url.Replace("{id:int}", trackId.ToString()))
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError($"track with id {trackId} doesn't exist")
                |> ignore
            }