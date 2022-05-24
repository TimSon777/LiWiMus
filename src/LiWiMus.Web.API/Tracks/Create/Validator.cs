using FluentValidation;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared.Extensions;

namespace LiWiMus.Web.API.Tracks.Create;

// ReSharper disable once UnusedType.Global
public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(r => r.Name)
            .NotNull()
            .Length(2, 50)
            .DisableTags();

        RuleFor(r => r.GenreIds)
            .NotEmpty()
            .WithMessage(ValidationMessages.MustHas("Track", "genres"));
        
        RuleFor(r => r.OwnerIds)
            .NotEmpty()
            .WithMessage(ValidationMessages.MustHas("Track", "owners (artists)"));
        
        RuleFor(r => r.PublishedAt)
            .Must(d => d <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage(ValidationMessages.DateLessThenNow);

        RuleFor(r => r.Duration)
            .GreaterThan(0);

        RuleFor(r => r.FileLocation)
            .NotEmpty()
            .MaximumLength(100);
    }
}