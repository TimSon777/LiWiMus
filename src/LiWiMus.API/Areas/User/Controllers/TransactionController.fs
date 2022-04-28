namespace LiWiMus.API.Areas.User.Controllers

open System.Threading.Tasks
open AutoMapper
open LiWiMus.API.Areas.User.Dto
open LiWiMus.Core.Transactions
open LiWiMus.SharedKernel.Interfaces
open Microsoft.AspNetCore.Mvc

[<Area("User")>]
type TransactionController(transactionRepository: IRepository<Transaction>, mapper: IMapper) =
    inherit Controller()

    [<HttpPost>]
    member this.Update([<FromBody>] transactionDto: TransactionDtoUpdate) : Task<IActionResult> =
        async {
            let! transaction =
                transactionRepository.GetByIdAsync(transactionDto.Id)
                |> Async.AwaitTask

            if transaction = null then
                return BadRequestObjectResult(transactionDto) :> IActionResult
            else
                mapper.Map(transactionDto, transaction) |> ignore

                let! _ =
                    transactionRepository.UpdateAsync(transaction)
                    |> Async.AwaitTask

                return OkObjectResult(transaction) :> IActionResult
        }
        |> Async.StartAsTask
