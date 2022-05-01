using FluentValidation;
using LiWiMus.Web.MVC.Areas.User.ViewModels;

namespace LiWiMus.Web.MVC.Areas.User.Validators;

public class PaymentValidator : AbstractValidator<PaymentViewModel>
{
    public PaymentValidator()
    {
        RuleFor(model => model.Amount)
            .GreaterThan(0);
    }
}