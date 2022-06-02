namespace LiWiMus.Web.API.Tests.UserPlans

open System
open LiWiMus.Core.Plans
open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open FluentAssertions
open LiWiMus.Web.Shared.Extensions
open LiWiMus.Web.API.Tests.UserPlans.TestsHelpers
open LiWiMus.Web.API.Tests.HttpResponseMessage
open System.Linq

type UpdateTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.UserPlans.Update
        
        static member SuccessMemberData: obj [] list = [
             [|440000; DateTime.MaxValue|]
        ]
        
        static member FailureNotFoundMemberData: obj [] list = [
             [|445000; DateTime.MaxValue|]
        ]
        
        interface IClassFixture<TestApplicationFactory>
    
        [<Theory>]
        [<MemberData(nameof(UpdateTests.SuccessMemberData))>]
        member this.``Tests(UserPlans): Create => Success``(userPlanId, endDate) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = UserPlans.Update.Request(End = endDate)
            
            task {
                
                //Act
                let! httpMessage = client.PatchAsJsonAsync(url.Replace("{id:int}", userPlanId), body)
                
                //Assert
                httpMessage
                    .Should()
                    .BeSuccessful("all params must be valid (see Validator)")
                |> ignore
            }
            
            
        [<Theory>]
        [<MemberData(nameof(UpdateTests.FailureNotFoundMemberData))>]
        member this.``Tests(UserPlans): Create => Failure (unprocessable entity - not found)``(userPlanId, endDate) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = UserPlans.Update.Request(End = endDate)
            
            task {
                
                //Act
                let! httpMessage = client.PatchAsJsonAsync(url.Replace("{id:int}", userPlanId), body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("cannot edit a non-existent plan")
                |> ignore
            }
            
        [<Fact>]
        member this.``Tests(UserPlans): Create => Failure (unprocessable entity - try edit default user's plan)``() =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = UserPlans.Update.Request(End = DateTime.MaxValue)
   
            let urlList = AddQueries RouteConstants.UserPlans.List "440000" null (null |> string)
            
            task {
                
                let! httpMessageListUserPlans = client.GetAsync(urlList)
                
                httpMessageListUserPlans
                    .Should()
                    .BeSuccessful("tests for user plan list must pass")
                |> ignore
                
                let! userPlans = httpMessageListUserPlans.ToArrayAsync<UserPlans.Dto>()
                let userPlanDto = (fun (x:UserPlans.Dto) -> x.PlanName = DefaultPlans.Default.Name)
                                 |> userPlans.FirstOrDefault
                
                userPlanDto
                    .Should()
                    .NotBeNull("each user must have a default plan")
                |> ignore
                
                //Act
                let! httpMessage = client.PatchAsJsonAsync(url.Replace("{id:int}", userPlanDto.Id |> string), body)
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("cannot edit the default plan")
                |> ignore
            }