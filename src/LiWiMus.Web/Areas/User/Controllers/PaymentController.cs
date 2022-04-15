using FormHelper;
using LiWiMus.Core.Exceptions;
using LiWiMus.Core.IdentityAggregates;
using LiWiMus.Core.IdentityAggregates.Specifications;
using LiWiMus.Core.Interfaces;
using LiWiMus.Infrastructure.Identity;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.Areas.User.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.Areas.User.Controllers;

[Area("User")]
public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;
    private readonly UserManager<UserIdentity> _userManager;
    private readonly IRepository<IdentityAggregate> _aggregateRepository;

    public PaymentController(IPaymentService paymentService, UserManager<UserIdentity> userManager, IRepository<IdentityAggregate> aggregateRepository)
    {
        _paymentService = paymentService;
        _userManager = userManager;
        _aggregateRepository = aggregateRepository;
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
        var identity = await _userManager.GetUserAsync(User);
        var aggregate = await _aggregateRepository.GetBySpecAsync(new IdentityAggregateByIdWithUserSpec(identity.Id));
        if (aggregate is null)
        {
            return BadRequest();
        }

        var user = aggregate.User;
        if (user is null)
        {
            return BadRequest();
        }

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