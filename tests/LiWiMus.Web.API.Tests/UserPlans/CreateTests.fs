namespace LiWiMus.Web.API.Tests.UserPlans

open System
open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open System.Net.Http.Json
open FluentAssertions

type CreateTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.UserPlans.Create
        
        static member SuccessMemberData: obj [] list = [
             [|380000; 2; DateTime.Now; DateTime.MaxValue|]
             [|380001; 2; DateTime(2000, 1, 1); DateTime.MaxValue|]
        ]
        
        static member FailureMemberData: obj [] list = [
             [|380002; 2; DateTime.Now; DateTime.UtcNow|]
             [|380003; 2; DateTime.Now; DateTime.MinValue|]
        ]
        
        interface IClassFixture<TestApplicationFactory>
    
        [<Theory>]
        [<MemberData(nameof(CreateTests.SuccessMemberData))>]
        member this.``Tests(UserPlans): Create => Success``(userId, planId, startDate, endDate) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = UserPlans.Create.Request(
                UserId = userId,
                PlanId = planId,
                Start = startDate,
                End = endDate)
            
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
        [<MemberData(nameof(CreateTests.FailureMemberData))>]
        member this.``Tests(UserPlans): Create => Failure (bad request)``(userId, planId, startDate, endDate) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = UserPlans.Create.Request(
                UserId = userId,
                PlanId = planId,
                Start = startDate,
                End = endDate)
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("must be bad request - params invalid (maybe requirements were changed?)")
                |> ignore
            }