using LiWiMus.Core.Entities;
using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Settings;
using LiWiMus.Core.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LiWiMus.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IRepository<UserPlan> _userPlanRepository;
    private readonly IRepository<Plan> _planRepository;
    private readonly IAvatarService _avatarService;
    private readonly IWebHostEnvironment _environment;
    private readonly IOptions<DataSettings> _dataDirectoriesOptions;

    private readonly HttpClient _httpClient = new();

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
                             IRepository<UserPlan> userPlanRepository, IRepository<Plan> planRepository,
                             IAvatarService avatarService, IWebHostEnvironment environment,
                             IOptions<DataSettings> dataDirectoriesOptions)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userPlanRepository = userPlanRepository;
        _planRepository = planRepository;
        _avatarService = avatarService;
        _environment = environment;
        _dataDirectoriesOptions = dataDirectoriesOptions;
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

        var defaultPlan = await _planRepository.GetBySpecAsync(new DefaultPlanSpecification()) ??
                          throw new SystemException();
        var userPlan = new UserPlan
        {
            Plan = defaultPlan,
            Start = DateTime.Now,
            End = DateTime.MaxValue
        };
        await _userPlanRepository.AddAsync(userPlan);
        var user = new User {Email = model.Email, UserName = model.UserName, UserPlan = userPlan};
        await _avatarService.SetRandomAvatarAsync(user, _httpClient, _environment.ContentRootPath,
            _dataDirectoriesOptions.Value.AvatarsDirectory);
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(model);
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
}