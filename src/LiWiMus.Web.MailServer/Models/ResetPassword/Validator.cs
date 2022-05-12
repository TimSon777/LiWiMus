using FluentValidation;

namespace LiWiMus.Web.MailServer.Models.ResetPassword;

// ReSharper disable once UnusedType.Global
public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(m => m.UserEmail)
            .NotNull()
            .EmailAddress();

        RuleFor(m => m.UserName)
            .NotNull()
            .Length(2, 100);

        RuleFor(m => m.ResetUrl)
            .NotNull();
    }
}