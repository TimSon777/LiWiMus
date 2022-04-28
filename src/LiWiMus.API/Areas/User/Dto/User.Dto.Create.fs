namespace LiWiMus.API.Areas.User.Dto

open System.ComponentModel.DataAnnotations

type UserDtoCreate(userName:string, email:string, password:string, emailConfirmed:bool) =
    [<Required; StringLength(20)>]
    member val UserName = userName with get, set
    
    [<Required; EmailAddress>]
    member val Email = email with get, set
    
    [<Required; StringLength(30)>]
    member val Password = password with get, set
    
    [<Required>]
    member val EmailConfirmed = emailConfirmed with get, set
    
    new() = UserDtoCreate("", "", "", false)