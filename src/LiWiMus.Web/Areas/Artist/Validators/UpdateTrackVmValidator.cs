using FluentValidation;
using LiWiMus.Web.Areas.Artist.ViewModels;
using LiWiMus.Web.Extensions;

namespace LiWiMus.Web.Areas.Artist.Validators;

public class UpdateTrackVmValidator : AbstractValidator<UpdateTrackViewModel>
{
    public UpdateTrackVmValidator()
    {
        RuleFor(model => model.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50)
            .DisableTags();

        RuleFor(model => model.Id)
            .NotEmpty();

        RuleFor(model => model.PublishedAt)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
        
        // TODO: validate file (format, size)
        RuleFor(model => model.File)
            .NotNull();

        RuleFor(model => model.ArtistsIds)
            .NotEmpty()
            .When(model => model.ArtistsIds is not null);

        RuleFor(model => model.GenresIds)
            .NotEmpty()
            .When(model => model.GenresIds is not null);
    }
}