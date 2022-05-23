using FluentValidation;
using LiWiMus.Web.Shared.Extensions;

namespace LiWiMus.Web.API.Plans.Update;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.Description)
            .MaximumLength(500)
            .DisableTags();

        RuleFor(request => request.PricePerMonth)
            .GreaterThanOrEqualTo(0);

        RuleFor(request => request.Id)
            .NotEmpty();
    }
}