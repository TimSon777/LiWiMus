using FormHelper;
using LiWiMus.Core.FollowingUsers;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.MVC.Areas.User.Controllers;

[Area("User")]
public class FollowingController : Controller
{
    private readonly IRepository<Core.Users.User> _userRepository;
    private readonly UserManager<Core.Users.User> _userManager;

    public FollowingController(IRepository<Core.Users.User> userRepository, UserManager<Core.Users.User> userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    [HttpPut]
    [FormValidator]
    public async Task<JsonResult> FollowUser(string username)
    {
        var currentUser = await _userManager.GetUserAsync(User) ?? throw new SystemException();
        var otherUser = await _userManager.FindByNameAsync(username) ??
                        throw new BadHttpRequestException("No user with this username");

        if (currentUser.Following.Any(x => x.FollowingId == otherUser.Id))
        {
            return FormResult.CreateErrorResult("You are already following this user");
        }

        currentUser.Following.Add(new FollowingUser
        {
            Follower = currentUser,
            Following = otherUser
        });
        await _userRepository.UpdateAsync(currentUser);

        return FormResult.CreateSuccessResult("You have successfully subscribed");
    }
}