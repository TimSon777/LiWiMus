namespace LiWiMus.API.Areas.User.Profiles

open AutoMapper
open LiWiMus.API.Areas.User.Dto
open LiWiMus.Core.Transactions

type TransactionProfile() as this =
    inherit Profile()
    do this.CreateMap<TransactionDtoUpdate, Transaction>()
           .ReverseMap()
       |> ignore
    
    do this.CreateMap<TransactionDtoRead, Transaction>()
            .ReverseMap()
       |> ignore
