module LiWiMus.Web.API.Tests.HttpResponseMessage

open System.Net.Http
open Newtonsoft.Json.Linq

type HttpResponseMessage with
    member public this.ToArrayAsync<'T>() =
        task {
            let! json = this.Content.ReadAsStringAsync()
            return JArray.Parse(json).ToObject<'T[]>()
        }
