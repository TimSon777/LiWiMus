namespace LiWiMus.API.Areas.User.Profiles

open AutoMapper
open LiWiMus.API.Areas.User.Dto
open LiWiMus.Core.Users
open Microsoft.FSharp.Core
open LiWiMus.API.Extensions

type UserProfile() as this =
    inherit Profile()
    do this.CreateMap<User, UserDtoReadOne>()
           .ForMemberFs((fun dto -> dto.Email), fun opt -> opt.MapFromFs(fun e -> e.Balance))
           .ReverseMap()
       |> ignore
    
    do this.CreateMap<User, UserDtoCreate>()
            .ReverseMap()
       |> ignore