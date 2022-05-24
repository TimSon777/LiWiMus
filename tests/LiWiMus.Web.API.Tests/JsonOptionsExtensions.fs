module LiWiMus.Web.API.Tests.JsonOptionsExtensions

open DateOnlyTimeOnly.AspNet.Converters
open Microsoft.AspNetCore.Http.Json

type JsonOptions with
    member public this.WithDateTimeOnly() =
        let options = JsonOptions()
        options.SerializerOptions.Converters.Add(DateOnlyJsonConverter())
        options.SerializerOptions.Converters.Add(TimeOnlyJsonConverter())
        options.SerializerOptions
