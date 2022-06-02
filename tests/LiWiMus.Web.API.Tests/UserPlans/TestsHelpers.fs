module LiWiMus.Web.API.Tests.UserPlans.TestsHelpers

open System
open System.Collections.Generic
open LiWiMus.Web.API.Tests.TestHelpers
open Microsoft.AspNetCore.WebUtilities

let AddQueries baseUrl userId planId active =
    
    let queries = [
        if userId |> isNotNull then
            yield KeyValuePair<string, string>(nameof(userId), userId)
        if planId |> isNotNull then
            yield KeyValuePair<string, string>(nameof(planId), planId)
        if active |> String.IsNullOrEmpty |> not then
            yield KeyValuePair<string, string>(nameof(active), active)      
    ] 
    
    QueryHelpers.AddQueryString(baseUrl, queries)