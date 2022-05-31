namespace LiWiMus.Web.API.Tests.Mocks

open System.Threading.Tasks
open LiWiMus.Core.Interfaces

type MockAvatarService() =
    interface IAvatarService with
        member this.SetRandomAvatarAsync(user) =
            user.AvatarLocation <- "MockLocation"
            Task.CompletedTask