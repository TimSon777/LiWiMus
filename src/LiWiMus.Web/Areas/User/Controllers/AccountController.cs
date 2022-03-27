﻿using System.Security.Claims;
using LiWiMus.Core.Interfaces;
using LiWiMus.SharedKernel.Extensions;
using LiWiMus.Web.Areas.User.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.Areas.User.Controllers;

[Area("User")]
[AllowAnonymous]
public class AccountController : Controller
{
    private readonly UserManager<Core.Entities.User> _userManager;
    private readonly SignInManager<Core.Entities.User> _signInManager;
    private readonly IMailService _mailService;

    public AccountController(UserManager<Core.Entities.User> userManager,
                             SignInManager<Core.Entities.User> signInManager, IMailService mailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
            AddErrors(result);
            return View(model);
        }

        await _userManager.UpdateAsync(user);

        await SendConfirmEmailAsync(user);
        await _signInManager.SignInAsync(user, false);
        return RedirectToAction("Index", "Home", new {area = ""});
    }

    [HttpGet]
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
            AddErrors(result);
            return View();
        }

        await _signInManager.SignInAsync(user, false);
        return Redirect("/User/Profile/Index");
    }

    //
    // POST: /Account/ExternalLogin
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string? returnUrl = null)
    {
        // Request a redirect to the external login provider.
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new {Area = "User", ReturnUrl = returnUrl});
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    //
    // GET: /Account/ExternalLoginCallback
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        returnUrl ??= Url.Action("Index", "Home", new {Area = ""});

        if (remoteError != null)
        {
            ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
            return View(nameof(Login));
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return RedirectToAction(nameof(Login));
        }

        // Sign in the user with this external login provider if the user already has a login.
        var result =
            await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        if (result.Succeeded)
        {
            // Update any authentication tokens if login succeeded
            await _signInManager.UpdateExternalAuthenticationTokensAsync(info);

            return LocalRedirect(returnUrl!);
        }

        // If the user does not have an account, then ask the user to create an account.
        return View("ExternalLoginConfirmation",
            new ExternalLoginConfirmationViewModel
            {
                ReturnUrl = returnUrl,
                ProviderDisplayName = info.ProviderDisplayName
            });
    }

    //
    // POST: /Account/ExternalLoginConfirmation
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model,
                                                               string? returnUrl = null)
    {
        returnUrl ??= Url.Action("Index", "Home", new {Area = ""})!;

        if (ModelState.IsValid)
        {
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return View("ExternalLoginFailure");
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = new Core.Entities.User {UserName = model.UserName, Email = email};
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);

                    // Update any authentication tokens as well
                    await _signInManager.UpdateExternalAuthenticationTokensAsync(info);

                    await SendConfirmEmailAsync(user);

                    return LocalRedirect(returnUrl);
                }
            }
            AddErrors(result);
        }

        model.ReturnUrl = returnUrl;
        return View(model);
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    public IActionResult Denied(string returnUrl)
    {
        return View();
    }
}