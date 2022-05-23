using FluentValidation;
using LiWiMus.Web.Shared.Extensions;

namespace LiWiMus.Web.API.Roles.Update;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.Description)
            .MaximumLength(500)
            .DisableTags();

        RuleFor(request => request.Id)
            .NotEmpty();
    }
}