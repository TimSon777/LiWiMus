namespace LiWiMus.API.Areas.User.Dto

open LiWiMus.API.Areas.Artists.Dto
open System
open Microsoft.FSharp.Core

type UserDtoReadOne() =
    member val Id = 0 with get, set
    
    member val UserName = "" with get, set
    
    member val Email = "" with get, set
    
    member val EmailConfirmed = false with get, set
    
    member val Balance = 0.0 |> decimal with get, set
    
    member val BirthDate = Some DateOnly.MinValue with get, set
    
    member val FirstName = Some "" with get, set
    
    member val SecondName = Some "" with get, set
    
    member val Patronymic = Some "" with get, set
    
    member val Artists = Array.Empty<ArtistDtoReadList> with get, set
