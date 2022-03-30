using FluentValidation;
using LiWiMus.Web.Areas.User.ViewModels;

namespace LiWiMus.Web.Areas.User.Validators;

public class PaymentValidator : AbstractValidator<PaymentViewModel>
{
    public PaymentValidator()
    {
        RuleFor(model => model.Amount)
            .GreaterThan(0);
    }
}