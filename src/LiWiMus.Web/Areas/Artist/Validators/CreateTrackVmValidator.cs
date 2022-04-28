using FluentValidation;
using LiWiMus.Web.Areas.Artist.ViewModels;
using LiWiMus.Web.Extensions;

namespace LiWiMus.Web.Areas.Artist.Validators;

public class CreateTrackVmValidator : AbstractValidator<CreateTrackViewModel>
{
    public CreateTrackVmValidator()
    {
        RuleFor(model => model.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50)
            .DisableTags();

        RuleFor(model => model.AlbumId)
            .NotEmpty();

        RuleFor(model => model.PublishedAt)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
        
        // TODO: validate file (format, size)
        RuleFor(model => model.File)
            .NotNull();
    }
}