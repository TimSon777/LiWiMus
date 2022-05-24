namespace LiWiMus.Web.API.Tests.Albums

open System
open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open System.Net.Http.Json
open FluentAssertions
open Microsoft.AspNetCore.Http.Json
open LiWiMus.Web.API.Tests.JsonOptionsExtensions

type ReadTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.Albums.Read
        interface IClassFixture<TestApplicationFactory>
    
        [<Theory>]
        [<InlineData(90000)>]
        [<InlineData(90001)>]
        member this.``Tests(Albums): Read => Success``(id: int) =
            
            // Arrange
            let client = factory.CreateClient()
            
            task {
                
                //Act
                let! httpMessage = client.GetAsync(url.Replace("{id:int}", id.ToString()))
                
                //Assert
                httpMessage
                    .Should()
                    .BeSuccessful($"album with id {id} must exists (see seeder)")
                    |> ignore
            }
        [<Theory>]
        [<InlineData(95000)>]
        [<InlineData(95001)>]
        [<InlineData(95002)>]
        [<InlineData(95003)>]
        [<InlineData(95004)>]
        member this.``Tests(Albums): Read => Failure (unprocessable entity)``(id: int) =
            
            // Arrange
            let client = factory.CreateClient()
            
            task {
                
                //Act
                let! httpMessage = client.GetAsync(url.Replace("{id:int}", id.ToString()))
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError($"album with id {id} doesn't exist (see seeder)")
                    |> ignore
            }