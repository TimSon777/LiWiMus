using ByteSizeLib;
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
            .Length(2, 50)
            .DisableTags();
        
        RuleFor(r => r.PublishedAt)
            .Must(d => d is null || d <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage(ValidationConstants.DateLessThenNow);
        
        RuleFor(r => r.Cover)
            .SidesPercentageDifferenceMustBeLessThan(10)
            .MustWeightLessThan(ByteSize.FromMegaBytes(1));
    }
}