namespace LiWiMus.Web.API.Tests

open System.Collections.Generic
open Microsoft.AspNetCore.WebUtilities

module TestHelpers =
    let GenerateUrl(baseUrl:string, id:int, replace:string, page:int, itemsPerPage:int) =
        let queries = [
            KeyValuePair<string, string>("page", page.ToString())
            KeyValuePair<string, string>("itemsPerPage", itemsPerPage.ToString())
        ]
        
        QueryHelpers.AddQueryString(baseUrl.Replace(replace, id.ToString()), queries)
        
    let isNotNull x =
        x |> isNull |> not