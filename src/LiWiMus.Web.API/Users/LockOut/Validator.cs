using FluentValidation;
using LiWiMus.Web.API.Shared;

namespace LiWiMus.Web.API.Users.LockOut;

// ReSharper disable once UnusedType.Global
public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.End)
            .NotEmpty()
            .Must(r => r > DateTime.Now)
            .WithMessage(ValidationMessages.DateGreaterThenNow);
    }
}