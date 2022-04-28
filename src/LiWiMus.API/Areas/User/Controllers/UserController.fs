namespace LiWiMus.API.Areas.User.Controllers

open System.Threading.Tasks
open AutoMapper
open LiWiMus.API.Areas.User.Dto
open LiWiMus.Core.Users
open LiWiMus.Core.Users.Specifications
open LiWiMus.SharedKernel.Interfaces
open Microsoft.AspNetCore.Identity
open Microsoft.AspNetCore.Mvc

[<ApiController>]
[<Route("api/[area]/[controller]/[action]")>]
type UserController(mapper: IMapper,
                    userManager: UserManager<User>,
                    userRepository: IRepository<User>) =
    inherit ControllerBase()
    
//    [<HttpPost>]
//    member this.Create(userDto: UserDtoCreate) : Task<IActionResult> =
//        async {
//            let! user = userDto.UserName |> userManager.FindByNameAsync |> Async.AwaitTask
//            if user |> isNull = false then
//                return ConflictResult()
//            else
//                user = mapper.Map<User>(userDto)
//                let! result = userManager.CreateAsync(user, userDto.Password) |> Async.AwaitTask
//                if result.Succeeded then
//                    let createdUser = userRepository.GetBySpecAsync(UserWithIncludes(user, nameof(user.Artists)))
//                    let createdUserDto = mapper.Map<UserDtoReadOne>(user)
//                    return CreatedResult("", user)
//                else
//                    return 
//        } |> Async.StartAsTask