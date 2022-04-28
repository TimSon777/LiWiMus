namespace LiWiMus.API.Areas.Artists.Profiles

open AutoMapper
open LiWiMus.API.Areas.Artists.Dto
open LiWiMus.Core.Artists

type UserProfile() as this =
    inherit Profile()
    do this.CreateMap<Artist, ArtistDtoReadList>()
           .ReverseMap()
       |> ignore
