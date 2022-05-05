using FluentValidation;

namespace LiWiMus.Web.API.Users.Create;

// ReSharper disable once UnusedType.Global
public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(u => u.UserName)
            .NotEmpty()
            .Length(3, 20);

        RuleFor(u => u.Email)
            .NotEmpty()
            .Length(1, 256);

        RuleFor(u => u.Password)
            .NotEmpty()
            .Length(1, 100);
    }
}