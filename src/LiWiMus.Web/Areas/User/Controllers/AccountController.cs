using System.Security.Claims;
using LiWiMus.Core.Interfaces;
using LiWiMus.SharedKernel.Extensions;
using LiWiMus.Web.Areas.User.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.Areas.User.Controllers;

[Area("User")]
public class AccountController : Controller
{
    private readonly UserManager<Core.Entities.User> _userManager;
    private readonly SignInManager<Core.Entities.User> _signInManager;
    private readonly IAvatarService _avatarService;
    private readonly IWebHostEnvironment _environment;
    private readonly IMailService _mailService;

    private readonly HttpClient _httpClient = new();

    public AccountController(UserManager<Core.Entities.User> userManager, SignInManager<Core.Entities.User> signInManager,
                             IAvatarService avatarService, IWebHostEnvironment environment, IMailService mailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _avatarService = avatarService;
        _environment = environment;
        _mailService = mailService;
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

        var user = new Core.Entities.User {Email = model.Email, UserName = model.UserName};
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            result.Errors.Foreach(error => ModelState.AddModelError("", error.Description));

            return View(model);
        }

        await _avatarService.SetRandomAvatarAsync(user, _httpClient, _environment.ContentRootPath);
        await _userManager.UpdateAsync(user);

        await SendConfirmEmailAsync(user);
        await _signInManager.SignInAsync(user, false);
        return RedirectToAction("Index", "Home", new {area = ""});
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
            return RedirectToAction("Index", "Home", new {area = ""});
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
            return RedirectToAction("Index", "Home", new {area = ""});
        }

        ModelState.AddModelError("", "Неправильный логин и (или) пароль");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home", new {area = ""});
    }

    private async Task SendConfirmEmailAsync(Core.Entities.User user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var confirmUrl = Url.Action(
            "ConfirmEmail",
            "Account",
            new {userId = user.Id, code = token},
            HttpContext.Request.Scheme);

        await _mailService.SendConfirmEmailAsync(user.UserName, user.Email, confirmUrl!);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(string userName)
    {
        if (userName.IsNullOrEmpty())
        {
            return BadRequest("Введите имя пользователя");
        }

        var user = await _userManager.FindByNameAsync(userName);

        if (user is null)
        {
            return BadRequest("Пользователь не найден");
        }

        if (!user.EmailConfirmed)
        {
            return BadRequest("Ваш email не подтвержден");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var resetUrl = Url.Action(
            "ResetPasswordCallback",
            "Account",
            new {area = "User", userId = user.Id, token},
            HttpContext.Request.Scheme);

        await _mailService.SendResetPasswordAsync(userName, user.Email, resetUrl!);

        return Ok("Проверьте почтовый ящик");
    }

    [HttpGet]
    public async Task<IActionResult> ResetPasswordCallback(string userId, string token)
    {
        if (userId.IsNullOrEmpty() || token.IsNullOrEmpty())
        {
            return View("Error");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return View("Error");
        }

        var resetPasswordVm = new ResetPasswordViewModel
        {
            UserId = userId,
            Token = token
        };

        return View(resetPasswordVm);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPasswordCallback(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var user = await _userManager.FindByIdAsync(model.UserId);

        if (user is null)
        {
            return new NotFoundResult();
        }

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

        if (!result.Succeeded)
        {
            result.Errors.Foreach(error => ModelState.AddModelError("", error.Description));
            return View();
        }

        await _signInManager.SignInAsync(user, false);
        return Redirect("/User/Profile/Index");
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult ExternalLogin(string provider, string returnUrl)
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
            new {area = "User", ReturnUrl = returnUrl});

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
            user = new Core.Entities.User
            {
                UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                Email = info.Principal.FindFirstValue(ClaimTypes.Email)
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await _avatarService.SetRandomAvatarAsync(user, _httpClient, _environment.ContentRootPath);
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

    public IActionResult Denied(string returnUrl)
    {
        return View();
    }
}