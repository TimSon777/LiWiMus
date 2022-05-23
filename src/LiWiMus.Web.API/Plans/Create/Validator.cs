using FluentValidation;
using LiWiMus.Core.Plans;
using LiWiMus.Core.Plans.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.Shared.Extensions;

namespace LiWiMus.Web.API.Plans.Create;

public class Validator : AbstractValidator<Request>
{
    public Validator(IRepository<Plan> repository)
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(50)
            .DisableTags()
            .MustAsync(async (s, token) => !await repository.AnyAsync(new PlanByNameSpec(s), token))
            .WithMessage("Plan with this name already exists");

        RuleFor(request => request.Description)
            .NotEmpty()
            .MaximumLength(500)
            .DisableTags();

        RuleFor(request => request.PricePerMonth)
            .GreaterThanOrEqualTo(0);
    }
}