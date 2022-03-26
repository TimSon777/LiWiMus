using AutoMapper;
using FormHelper;
using LiWiMus.SharedKernel;
using LiWiMus.Web.Areas.User.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.Areas.User.Controllers;

[Area("User")]
[Route("[area]/[controller]/[action]")]
public class ProfileController : Controller
{
    private readonly UserManager<Core.Entities.User> _userManager;
    private readonly IMapper _mapper;

    public ProfileController(UserManager<Core.Entities.User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{userName?}")]
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
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [Authorize]
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

    [Authorize]
    [HttpPost]
    [FormValidator]
    public async Task<IActionResult> UpdateAsync(ProfileViewModel model)
    {
        model.BirthDate = DateOnly.TryParse(Request.Form[nameof(model.BirthDate)], out var birthDate) 
            ? birthDate 
            : null;
        
        var user = await _userManager.GetUserAsync(User);

        _mapper.Map(model, user);
        
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded 
            ? FormResult.CreateSuccessResult("Ok") 
            : FormResult.CreateErrorResult("Fu");
    }
}