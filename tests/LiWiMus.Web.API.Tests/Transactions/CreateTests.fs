namespace LiWiMus.Web.API.Tests.Transactions

open System
open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open System.Net.Http.Json
open FluentAssertions
open Microsoft.AspNetCore.Http.Json
open LiWiMus.Web.API.Tests.JsonOptionsExtensions
open LiWiMus.Web.API.Tests.Transactions.Generators

type CreateTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.Transactions.Create
        interface IClassFixture<TestApplicationFactory>
    
        [<Theory>]
        [<ClassData(typeof<UpdateTestsGeneratorSuccess>)>]
        member this.``Tests(Transactions): Create => Success``(amount) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Transactions.Create.Request(
                UserId = 40000,
                Amount = amount,
                Description = "Description"
            )
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .BeSuccessful("all params must be valid (see Validator)")
                    |> ignore
            }
            
            
        [<Theory>]
        [<ClassData(typeof<UpdateTestsGeneratorFailure>)>]
        member this.``Tests(Transactions): Create => Failure (bad request)``(description) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Transactions.Create.Request(
                UserId = 40000,
                Amount = 100,
                Description = description
            )
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("all params must be valid (see Validator)")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Transactions): Create => Failure (bad request - amount)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Transactions.Create.Request(
                UserId = 40000,
                Amount = 0,
                Description = "Description"
            )
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("amount can't be 0 (see Validator)")
                    |> ignore
            }
            
        [<Fact>]
        member this.``Tests(Transactions): Create => Failure (unprocessable entity)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Transactions.Create.Request(
                UserId = 45000,
                Amount = 10,
                Description = "Description"
            )
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("request to not existing entity")
                    |> ignore
            }