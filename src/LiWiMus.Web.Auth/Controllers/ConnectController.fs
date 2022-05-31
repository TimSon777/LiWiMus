namespace LiWiMus.Web.Auth.Controllers.ConnectController

open System
open System.Collections.Generic
open System.Security.Claims
open System.Threading.Tasks
open LiWiMus.Core.Users
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Identity
open Microsoft.AspNetCore.Mvc
open Microsoft.FSharp.Control
open OpenIddict.Abstractions
open OpenIddict.Server.AspNetCore

open type OpenIddictConstants.Permissions
open type OpenIddictServerAspNetCoreConstants
open Microsoft.AspNetCore

type Status =
   | Lockout = 0
   | Ok = 1
   | Error = 2
   
type ConnectController(userManager: UserManager<User>, _signInManager: SignInManager<User>) =
    inherit Controller()

    member private this.GetDestinations(principal: ClaimsPrincipal, claim: Claim) =
        seq {
            match claim.Type with
            | OpenIddictConstants.Claims.Name ->
                yield OpenIddictConstants.Destinations.AccessToken

                if principal.HasScope(Scopes.Profile) then
                    yield OpenIddictConstants.Destinations.IdentityToken
            | OpenIddictConstants.Claims.Role ->
                yield OpenIddictConstants.Destinations.AccessToken

                if principal.HasScope(Scopes.Roles) then
                    yield OpenIddictConstants.Destinations.IdentityToken
            | OpenIddictConstants.Claims.Email ->
                yield OpenIddictConstants.Destinations.AccessToken

                if principal.HasScope(Scopes.Email) then
                    yield OpenIddictConstants.Destinations.IdentityToken
            | "AspNet.Identity.SecurityStamp" -> "" |> ignore
            | _ -> yield OpenIddictConstants.Destinations.AccessToken
        }
        
    member private this.LockoutResult() : IActionResult =
        let errors = Dictionary<string, string>()
        errors.Add(Properties.Error, OpenIddictConstants.Errors.AccessDenied)
        errors.Add(Properties.ErrorDescription, "Account is blocked")
    
        let properties =
            AuthenticationProperties(errors)
    
        this.Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)
       
    member private this.ForbidResult() : IActionResult = 
        let errors = Dictionary<string, string>()
        errors.Add(Properties.Error, OpenIddictConstants.Errors.InvalidGrant)
        errors.Add(Properties.ErrorDescription, "The username/password couple is invalid.")
    
        let properties =
            AuthenticationProperties(errors)
    
        this.Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme) :> IActionResult
        
    member private this.SuccessResultAsync(request: OpenIddictRequest, user) =
        task {
            let! principal = _signInManager.CreateUserPrincipalAsync(user)
            let scopes =
                Set.intersect
                    (set [ Scopes.Email
                           Scopes.Profile
                           Scopes.Roles ])
                    (set (request.GetScopes() |> List.ofSeq))
        
            principal.SetScopes(scopes) |> ignore
        
            if String.IsNullOrEmpty(principal.FindFirstValue(OpenIddictConstants.Claims.Subject)) then
                principal.SetClaim(OpenIddictConstants.Claims.Subject, user.Id |> string)
                |> ignore
        
            for claim in principal.Claims do
                claim.SetDestinations(this.GetDestinations(principal, claim))
                |> ignore
        
            return this.SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme) :> IActionResult
        }

    member private this.TokenByPasswordAsync(request: OpenIddictRequest) =
        task {
            
            let! user = userManager.FindByNameAsync(request.Username)
         
            let status =
                if user = null then
                    Status.Error
                else
                    let result = _signInManager.CheckPasswordSignInAsync(user, request.Password, true)
                               |> Async.AwaitTask
                               |> Async.RunSynchronously
                               
                    if result.IsLockedOut then 
                        Status.Lockout
                    elif result.Succeeded then
                        Status.Ok
                    else
                        Status.Error
    
            match status with
            | Status.Error ->
                return this.ForbidResult()
            | Status.Ok ->
                return! this.SuccessResultAsync(request, user)
            | Status.Lockout -> 
                return this.LockoutResult()
            | _ ->
                return ArgumentOutOfRangeException() |> raise
        } : Task<IActionResult>
        

    [<HttpPost; Produces("application/json")>]
    member this.Token() =
        let request =
            this.HttpContext.GetOpenIddictServerRequest()

        task {
            if request.IsPasswordGrantType() then
                return! this.TokenByPasswordAsync(request)
            else
                return this.BadRequest() 
        }
