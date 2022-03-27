using AutoMapper;
using FormHelper;
using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Settings;
using LiWiMus.Web.Areas.User.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LiWiMus.Web.Areas.User.Controllers;

[Area("User")]
[Route("[area]/[controller]/[action]")]
public class ProfileController : Controller
{
    private readonly UserManager<Core.Entities.User> _userManager;
    private readonly IMapper _mapper;
    private readonly IAvatarService _avatarService;
    private readonly string _contentRootPath;

    public ProfileController(UserManager<Core.Entities.User> userManager, 
        IMapper mapper,
        IHostEnvironment environment,
        IAvatarService avatarService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _avatarService = avatarService;
        _contentRootPath = environment.ContentRootPath;
    }

    [HttpGet]
    [Route("{userName?}")]
    [AllowAnonymous]
    public async Task<IActionResult> Index(string userName)
    {
        ModelState.Clear();
        var currentUser = await _userManager.GetUserAsync(User);

        var user = string.IsNullOrEmpty(userName)
            ? currentUser
            : await _userManager.FindByNameAsync(userName);


        if (user is null)
        {
            return View("Error");
        }

        var profile = _mapper.Map<ProfileViewModel>(user);
        profile.IsAccountOwner = currentUser == user;

        return View(profile);
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var user = await _userManager.GetUserAsync(User);

        if (user != null)
        {
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Profile", new {area = "User"});
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, "User not found");
        }

        return View();
    }

    [HttpPost]
    [FormValidator]
    public async Task<IActionResult> UpdateAsync(ProfileViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);

        _mapper.Map(model, user);

        if (model.Avatar is not null)
        {
            await _avatarService.SetAvatarAsync(user, model.Avatar, _contentRootPath);
        }

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded 
            ? FormResult.CreateSuccessResult("Ok") 
            : FormResult.CreateErrorResult("Fu");
    }
}