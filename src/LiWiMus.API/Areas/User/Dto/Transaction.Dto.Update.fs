namespace LiWiMus.API.Areas.User.Dto

type TransactionDtoUpdate(id:int, description:string) =
    member val Id = id with get, set
    member val Description = description with get, set
    new() = TransactionDtoUpdate(0, "")
    