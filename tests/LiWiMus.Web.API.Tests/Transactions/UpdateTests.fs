namespace LiWiMus.Web.API.Tests.Transactions

open LiWiMus.Web.API.Shared
open LiWiMus.Web.API
open LiWiMus.Web.API.Tests
open Xunit
open LiWiMus.Web.Shared.Extensions
open FluentAssertions

type UpdateTests(factory: TestApplicationFactory) =
    let url = RouteConstants.Transactions.Update
    interface IClassFixture<TestApplicationFactory>
    
    [<Theory>]
    [<InlineData(1000, "Description")>]
    [<InlineData(1000, "Longgggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg")>]
    [<InlineData(1000, "Description_2")>]
    member this.``Tests(Transactions): Update => Success``(id, description) =

        // Arrange
        let client = factory.CreateClient()

        let body =
            Transactions.Update.Request(
                Id = id,
                Description = description
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
    member this.``Tests(Transactions): Update => Failure (not found)``() =

        // Arrange
        let client = factory.CreateClient()

        let body =
            Transactions.Update.Request(
                Id = 100000,
                Description = "Description"
            )

        task {

            //Act
            let! httpMessage = client.PatchAsJsonAsync(url, body)

            //Assert
            httpMessage
                .Should()
                .HaveClientError("transaction does not exist")
            |> ignore
        }
        
        
    [<Theory>]
    [<InlineData(100, null)>]
    [<InlineData(100, "VeryLonggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg")>]
    member this.``Tests(Transactions): Update => Failure (description empty / very long)``(id, description) =

        // Arrange
        let client = factory.CreateClient()

        let body =
            Transactions.Update.Request(
                Id = id,
                Description = description
            )

        task {

            //Act
            let! httpMessage = client.PatchAsJsonAsync(url, body)

            //Assert
            httpMessage
                .Should()
                .HaveClientError("params do not valid")
            |> ignore
        }