namespace LiWiMus.API.Areas.User.Dto

type TransactionDto(id:int, description:string) =
    member val Id = id with get, set
    member val Description = description with get, set
    new() = TransactionDto(0, "")
    