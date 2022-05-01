module Tests

open Xunit

//endpoints.MapControllerRoute("default", "api/{controller}/{action}/{id?}")
//endpoints.MapControllerRoute("MyArea", "api/{area:exists}/{controller}/{action}/{id?}")

[<Fact>]
let ``My test`` () = Assert.True(true)

let controllerWithoutArea =
    "https://localhost:7021/api/TestWithoutArea/TestGet"
