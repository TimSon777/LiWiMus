namespace LiWiMus.API.Areas.User.Profiles

open AutoMapper
open LiWiMus.API.Areas.User.Dto
open LiWiMus.Core.Transactions

type TransactionProfile() as this =
    inherit Profile()
    do this.CreateMap<TransactionDto, Transaction>()
           .ReverseMap()
       |> ignore
    
    do this.CreateMap<TransactionWithAmountDto, Transaction>()
            .ReverseMap()
       |> ignore
