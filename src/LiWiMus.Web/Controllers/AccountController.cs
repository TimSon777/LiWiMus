using LiWiMus.Core.Entities;
using LiWiMus.Core.Interfaces;
using LiWiMus.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IAvatarService _avatarService;
    private readonly IWebHostEnvironment _environment;
    private readonly IMailService _mailService;
    private readonly IPlanService _planService;

    private readonly HttpClient _httpClient = new();

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
                             IAvatarService avatarService, IWebHostEnvironment environment, IMailService mailService, IPlanService planService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _avatarService = avatarService;
        _environment = environment;
        _mailService = mailService;
        _planService = planService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new User {Email = model.Email, UserName = model.UserName};
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        await _planService.SetDefaultPlanAsync(user);
        await _avatarService.SetRandomAvatarAsync(user, _httpClient, _environment.ContentRootPath);
        await _userManager.UpdateAsync(user);

        await SendConfirmEmailAsync(user);
        await _signInManager.SignInAsync(user, false);
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmailAsync(string userId, string code)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
        {
            return View("Error");
        }
        
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null)
        {
            return View("Error");
        }
        
        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        return View("Error");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result =
            await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Неправильный логин и (или) пароль");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    
    private async Task SendConfirmEmailAsync(User user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var confirmUrl = Url.Action(
            "ConfirmEmail", 
            "Account", 
            new { userId = user.Id, code = token },
            HttpContext.Request.Scheme);

        await _mailService.SendConfirmEmailAsync(user.UserName, user.Email, confirmUrl!);
    }
}