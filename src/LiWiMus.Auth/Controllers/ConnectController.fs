namespace LiWiMus.Auth.Controllers

open System.Collections.Generic
open System.Security.Claims
open LiWiMus.Core.Users
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Identity
open Microsoft.AspNetCore.Mvc
open OpenIddict.Abstractions
open OpenIddict.Server.AspNetCore
open type OpenIddictConstants.Permissions
open type OpenIddictServerAspNetCoreConstants;
open Microsoft.AspNetCore

type ConnectController (userManager: UserManager<User>, _signInManager : SignInManager<User>) =
    inherit Controller()
    
    member private this.GetDestinations(principal:ClaimsPrincipal, claim:Claim) =
        seq {
            match claim.Type with
            | OpenIddictConstants.Claims.Name ->
                yield OpenIddictConstants.Destinations.AccessToken
                if principal.HasScope(Scopes.Profile) then
                    yield OpenIddictConstants.Destinations.IdentityToken;
            | OpenIddictConstants.Claims.Role ->
                yield OpenIddictConstants.Destinations.AccessToken
                if principal.HasScope(Scopes.Roles) then
                    yield OpenIddictConstants.Destinations.IdentityToken;
            | OpenIddictConstants.Claims.Email ->
                yield OpenIddictConstants.Destinations.AccessToken
                if principal.HasScope(Scopes.Email) then
                    yield OpenIddictConstants.Destinations.IdentityToken;
            | "AspNet.Identity.SecurityStamp" -> "" |> ignore
            | _ ->
                yield OpenIddictConstants.Destinations.AccessToken
        }
    
    member private this.TokenByPassword(request : OpenIddictRequest) : IActionResult =
        let user = userManager.FindByNameAsync(request.Username)
                   |> Async.AwaitTask
                   |> Async.RunSynchronously
        
        let isOk =
            if user = null then
                false
            else
                let result = _signInManager.CheckPasswordSignInAsync(user, request.Password, true)
                             |> Async.AwaitTask
                             |> Async.RunSynchronously
                result.Succeeded
        
        if isOk = false then
            let errors = Dictionary<string, string>()
            errors.Add(Properties.Error, OpenIddictConstants.Errors.InvalidGrant)
            errors.Add(Properties.ErrorDescription, "The username/password couple is invalid.")
            let properties = AuthenticationProperties(errors)
            this.Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)
        else
            let principal = _signInManager.CreateUserPrincipalAsync(user)
                             |> Async.AwaitTask
                             |> Async.RunSynchronously
            let scopes = Set.intersect (set [
                Scopes.Email
                Scopes.Profile
                Scopes.Roles ]) ( set (request.GetScopes() |> List.ofSeq))
            
            principal.SetScopes(scopes) |> ignore
            if System.String.IsNullOrEmpty(principal.FindFirstValue(OpenIddictConstants.Claims.Subject)) then
                principal.SetClaim(OpenIddictConstants.Claims.Subject, user.Id |> string) |> ignore;
            for claim in principal.Claims do
                claim.SetDestinations(this.GetDestinations(principal, claim)) |> ignore
            
            this.SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)
    
    [<HttpPost; Produces("application/json")>]
    member this.Token() =
        let request = this.HttpContext.GetOpenIddictServerRequest()
        
        if request.IsPasswordGrantType() then
            this.TokenByPassword(request)
        else
            this.BadRequest()