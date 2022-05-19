using FluentValidation;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared.Extensions;

namespace LiWiMus.Web.API.Albums.Update;

// ReSharper disable once UnusedType.Global
public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(r => r.Title)
            .Length(5, 50)
            .DisableTags();

        RuleFor(r => r.PublishedAt)
            .Cascade(CascadeMode.Stop)
            .Must(d => d <= DateOnly.FromDateTime(DateTime.UtcNow))
            .When(request => request.PublishedAt is not null, ApplyConditionTo.CurrentValidator)
            .WithMessage(ValidationMessages.DateLessThenNow);
    }
}