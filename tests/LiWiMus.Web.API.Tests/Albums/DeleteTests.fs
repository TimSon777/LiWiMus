namespace LiWiMus.Web.API.Tests.Albums

open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions

type DeleteTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.Albums.Delete
        interface IClassFixture<TestApplicationFactory>
    
        [<Theory>]
        [<InlineData(90000)>]
        [<InlineData(90001)>]
        member this.``Tests(Albums): Delete => Success``(id: int) =
            
            // Arrange
            let client = factory.CreateClient()
            
            task {
                
                //Act
                let! httpMessage = client.DeleteAsync(url.Replace("{id:int}", id.ToString()))
                
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
        member this.``Tests(Albums): Delete => Failure (unprocessable entity)``(id) =
            
            // Arrange
            let client = factory.CreateClient()
            
            task {
                
                //Act
                let! httpMessage = client.DeleteAsync(url.Replace("{id:int}", id.ToString()))
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError($"album with id {id} doesn't exist (see seeder)")
                    |> ignore
            }
