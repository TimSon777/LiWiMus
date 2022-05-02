﻿#region

using AutoMapper;
using FormHelper;
using LiWiMus.Core.Interfaces;
using LiWiMus.Web.MVC.Areas.User.ViewModels;
using LiWiMus.Web.Shared.Extensions;
using LiWiMus.Web.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace LiWiMus.Web.MVC.Areas.User.Controllers;

[Area("User")]
[Route("[area]/[controller]")]
public class ProfileController : Controller
{
    private readonly IFormFileSaver _formFileSaver;
    private readonly IAvatarService _avatarService;
    private readonly IMapper _mapper;
    private readonly UserManager<Core.Users.User> _userManager;

    public ProfileController(UserManager<Core.Users.User> userManager,
                             IMapper mapper, IAvatarService avatarService, IFormFileSaver formFileSaver)
    {
        _userManager = userManager;
        _mapper = mapper;
        _avatarService = avatarService;
        _formFileSaver = formFileSaver;
    }

    [HttpGet("{userName?}")]
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

    [HttpGet("[action]/{userName?}")]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost("[action]")]
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

    [HttpPost("[action]")]
    [FormValidator]
    public async Task<IActionResult> UpdateAsync(ProfileViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);

        _mapper.Map(model, user);

        if (model.Avatar is not null)
        {
            user.AvatarPath = await _formFileSaver.SaveWithRandomNameAsync(model.Avatar);
        }

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? FormResult.CreateSuccessResult("Ok")
            : FormResult.CreateErrorResult("Fu");
    }

    [HttpPost("[action]")]
    [FormValidator]
    public async Task<IActionResult> ChangeAvatarToRandom()
    {
        var user = await _userManager.GetUserAsync(User);
        await _avatarService.SetRandomAvatarAsync(user);
        await _userManager.UpdateAsync(user);
        return FormResult.CreateSuccessResult("Refresh the page (ctrl f5) for the changes to take effect");
    }
}