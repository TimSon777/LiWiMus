using System.Security.Claims;
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
                             IAvatarService avatarService, IWebHostEnvironment environment, IMailService mailService,
                             IPlanService planService)
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
    public async Task<IActionResult> Login(string returnUrl)
    {
        return View(new LoginViewModel()
        {
            ReturnUrl = returnUrl,
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
        });
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
            new {userId = user.Id, code = token},
            HttpContext.Request.Scheme);

        await _mailService.SendConfirmEmailAsync(user.UserName, user.Email, confirmUrl!);
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult ExternalLogin(string provider, string returnUrl)
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
            new {ReturnUrl = returnUrl});

        var properties =
            _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

        return new ChallengeResult(provider, properties);
    }

    [AllowAnonymous]
    public async Task<IActionResult>
        ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        returnUrl ??= Url.Content("~/");

        var loginViewModel = new LoginViewModel
        {
            ReturnUrl = returnUrl,
            ExternalLogins =
                (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
        };

        if (remoteError != null)
        {
            ModelState
                .AddModelError("", $"Error from external provider: {remoteError}");

            return View("Login", loginViewModel);
        }

        // Get the login information about the user from the external login provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ModelState
                .AddModelError("", "Error loading external login information.");

            return View("Login", loginViewModel);
        }

        // If the user already has a login (i.e if there is a record in AspNetUserLogins
        // table) then sign-in the user with this external login provider
        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
            info.ProviderKey, false, true);

        if (signInResult.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        // If there is no record in AspNetUserLogins table, the user may not have
        // a local account

        // Get the email claim value
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);

        if (email == null)
        {
            ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
            ViewBag.ErrorMessage = "Please contact support on liwimus@aslipatov.site";

            return View("Error");
        }

        // If we cannot find the user email we cannot continue
        // Create a new user without password if we do not have a user already
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new User
            {
                UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                Email = info.Principal.FindFirstValue(ClaimTypes.Email)
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await _avatarService.SetRandomAvatarAsync(user, _httpClient, _environment.ContentRootPath);
                await _planService.SetDefaultPlanAsync(user);
                await _userManager.UpdateAsync(user);
                await SendConfirmEmailAsync(user);
            }
            else
            {
                ViewBag.ErrorTitle = $"{info.ProviderDisplayName} registration error";
                ViewBag.ErrorMessage = result.Errors.FirstOrDefault()?.Description ?? "";
                return View("Error");
            }
        }

        // Add a login (i.e insert a row for the user in AspNetUserLogins table)
        await _userManager.AddLoginAsync(user, info);
        await _signInManager.SignInAsync(user, false);

        return LocalRedirect(returnUrl);
    }
}