using FluentValidation;

namespace LiWiMus.Web.API.Users.LockOut;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.End)
            .NotEmpty();
    }
}