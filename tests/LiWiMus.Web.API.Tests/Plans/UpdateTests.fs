namespace LiWiMus.Web.API.Tests.Plans

open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions
open LiWiMus.Web.API
open LiWiMus.Web.Shared.Extensions
open LiWiMus.Web.API.Tests.Plans.Generators

type UpdateTests(factory: TestApplicationFactory) =
    let url = RouteConstants.Plans.Update
    interface IClassFixture<TestApplicationFactory>

    [<Theory>]
    [<ClassData(typeof<UpdateTestsGeneratorSuccess>)>]
    member this.``Tests(Plans): Update => Success``(id, description, pricePerMonth) =

        // Arrange
        let client = factory.CreateClient()

        let body = Plans.Update.Request(
            Id = id,
            Description = description,
            PricePerMonth = pricePerMonth
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
    member this.``Tests(Plans): Update => Success (send only id)``() =

        // Arrange
        let client = factory.CreateClient()

        let body =
            Plans.Update.Request(Id = 180000)

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
    [<ClassData(typeof<UpdateTestsGeneratorBadRequest>)>]
    member this.``Tests(Plans): Update => Failure (bad request)``(id, description, pricePerMonth) =

        // Arrange
        let client = factory.CreateClient()

        let body = Plans.Update.Request(
            Id = id,
            Description = description,
            PricePerMonth = pricePerMonth
        )

        task {

            //Act
            let! httpMessage = client.PatchAsJsonAsync(url, body)

            //Assert
            httpMessage
                .Should()
                .HaveClientError("not valid params (see validator)")
            |> ignore
        }
        
    [<Theory>]
    [<InlineData(185000)>]
    [<InlineData(185001)>]
    [<InlineData(185002)>]
    [<InlineData(185003)>]
    [<InlineData(185004)>]
    member this.``Tests(Plans): Update => Failure (not found)``(id) =

        // Arrange
        let client = factory.CreateClient()

        let body =
            Plans.Update.Request(Id = id)

        task {

            //Act
            let! httpMessage = client.PatchAsJsonAsync(url, body)

            //Assert
            httpMessage
                .Should()
                .HaveClientError("plan doesn't exist")
            |> ignore
        }
        