using FluentValidation;
using LiWiMus.Web.API.Shared;

namespace LiWiMus.Web.API.Albums.Create;

// ReSharper disable once UnusedType.Global
public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(r => r.Title)
            .NotEmpty()
            .Length(2, 50);

        RuleFor(r => r.PublishedAt)
            .Must(d => d <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage(ValidationMessages.DateLessThenNow);

        RuleFor(r => r.CoverLocation)
            .Length(1, 100);
    }
}