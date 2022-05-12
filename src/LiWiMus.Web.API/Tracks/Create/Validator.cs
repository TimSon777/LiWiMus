using FluentValidation;
using LiWiMus.Web.API.Shared;

namespace LiWiMus.Web.API.Tracks.Create;

// ReSharper disable once UnusedType.Global
public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(r => r.Name)
            .NotNull()
            .Length(2, 50);

        RuleFor(r => r.GenreIds)
            .Must(genres => genres.Any())
            .WithMessage(ValidationMessages.MustHas("Track", "genres"));
        
        RuleFor(r => r.OwnerIds)
            .Must(genres => genres.Any())
            .WithMessage(ValidationMessages.MustHas("Track", "owners (artists)"));
        
        RuleFor(r => r.PublishedAt)
            .Must(d => d <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage(ValidationMessages.DateLessThenNow);

        RuleFor(r => r.Duration)
            .GreaterThan(0);
    }
}