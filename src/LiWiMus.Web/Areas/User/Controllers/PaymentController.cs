using FormHelper;
using LiWiMus.Core.Exceptions;
using LiWiMus.Core.Interfaces;
using LiWiMus.Web.Areas.User.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.Areas.User.Controllers;

[Area("User")]
public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;
    private readonly UserManager<Core.Users.User> _userManager;

    public PaymentController(IPaymentService paymentService, UserManager<Core.Users.User> userManager)
    {
        _paymentService = paymentService;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Pay(string? returnUrl, string? reason, int amount = 100)
    {
        var model = new PaymentViewModel
        {
            Amount = amount,
            ReturnUrl = returnUrl ?? Url.Content("~/"),
            Reason = reason
        };
        return View(model);
    }

    [HttpPost]
    [FormValidator]
    public async Task<IActionResult> Pay(PaymentViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            await _paymentService.PayAsync(user, model.Amount, model.Reason);
        }
        catch (PaymentException)
        {
            return FormResult.CreateErrorResult("Payment error. Try again later");
        }

        return FormResult.CreateSuccessResult("Success", model.ReturnUrl, 0);
    }
}