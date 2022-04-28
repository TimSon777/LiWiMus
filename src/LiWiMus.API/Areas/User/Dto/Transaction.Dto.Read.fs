namespace LiWiMus.API.Areas.User.Dto

type TransactionDtoRead(id:int, description:string, amount:decimal) =
    inherit TransactionDtoUpdate(id, description)
    member val Amount = amount with get, set
    new() = TransactionDtoRead(0, "", 0)