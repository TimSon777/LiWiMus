namespace LiWiMus.API.Areas.User.Dto

type TransactionWithAmountDto(id:int, description:string, amount:decimal) =
    inherit TransactionDto(id, description)
    member val Amount = amount with get, set
    new() = TransactionWithAmountDto(0, "", 0)