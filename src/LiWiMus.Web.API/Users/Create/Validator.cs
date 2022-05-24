using FluentValidation;
using LiWiMus.Web.Shared.Extensions;

namespace LiWiMus.Web.API.Users.Create;

// ReSharper disable once UnusedType.Global
public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(u => u.UserName)
            .NotEmpty()
            .Length(3, 20)
            .DisableTags();

        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress()
            .Length(1, 256);

        RuleFor(u => u.Password)
            .NotEmpty()
            .MaximumLength(100);
    }
}