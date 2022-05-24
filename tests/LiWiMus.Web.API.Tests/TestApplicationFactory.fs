namespace LiWiMus.Web.API.Tests

open System.Collections.Generic
open System.Net.Http
open System.Net.Http.Headers
open Microsoft.AspNetCore.Mvc.Testing
open Newtonsoft.Json.Linq
open Microsoft.AspNetCore.Hosting

type TestApplicationFactory() =
    inherit BaseApplicationFactory<Program>()
//    
//    override this.ConfigureClient(client) =
//        let auth = { new WebApplicationFactory<LiWiMus.Web.Auth.Program>() with
//                       override this.ConfigureWebHost(builder) =
//                           builder.UseEnvironment("Testing") |> ignore }
//        let authClient = auth.CreateClient()
//        
//        let body = [
//            KeyValuePair<string, string>("password", "admin")
//            KeyValuePair<string, string>("username", "admin")
//            KeyValuePair<string, string>("grant_type", "password")
//        ]
//        
//        let a = auth.Server.BaseAddress
//        let response = authClient.PostAsync("auth/connect/token", new FormUrlEncodedContent(body)).Result
//        let json = response.Content.ReadAsStringAsync().Result
//        let token = (JObject.Parse(json)["access_token"]).ToObject<string>()
//        let tokenType = (JObject.Parse(json)["token_type"]).ToObject<string>()
//        
//        client.DefaultRequestHeaders.Authorization <- AuthenticationHeaderValue(tokenType, token)
//        
//        base.ConfigureClient(client)
        