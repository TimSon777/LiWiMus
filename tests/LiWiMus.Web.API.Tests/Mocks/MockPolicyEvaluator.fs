namespace LiWiMus.Web.API.Tests.Mocks

open System.Security.Claims
open System.Threading.Tasks
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authorization.Policy

type MockPolicyEvaluator() =
    interface IPolicyEvaluator with
        member this.AuthenticateAsync(_, _) =
            let principal = ClaimsPrincipal()
            let identity = ClaimsIdentity([])
            
            principal.AddIdentity(identity)
            Task.FromResult(AuthenticateResult.Success(AuthenticationTicket(principal, AuthenticationProperties(), "TestScheme")))
            
        member this.AuthorizeAsync(_, _, _, _) =
            Task.FromResult(PolicyAuthorizationResult.Success())
