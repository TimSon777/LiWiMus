using FluentValidation;

namespace LiWiMus.Web.API.UserPlans.Create;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.End)
            .NotEmpty()
            .Must(end => end > DateTime.UtcNow)
            .Must((request, end) => end > request.Start);

        RuleFor(request => request.Start)
            .NotEmpty();

        RuleFor(request => request.PlanId)
            .NotEmpty();
        RuleFor(request => request.UserId)
            .NotEmpty();
    }
}