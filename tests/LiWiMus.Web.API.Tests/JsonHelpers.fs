module LiWiMus.Web.API.Tests.JsonHelpers

open DateOnlyTimeOnly.AspNet.Converters
open Microsoft.AspNetCore.Http.Json

type JsonOptions with
    member public this.WithDateTimeOnly() =
        let options = JsonOptions()
        options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter())
        options.SerializerOptions.Converters.Add(new TimeOnlyJsonConverter())
        options.SerializerOptions
