using ByteSizeLib;
using FluentValidation;
using LiWiMus.Web.Areas.Artist.ViewModels;
using LiWiMus.Web.Extensions;

namespace LiWiMus.Web.Areas.Artist.Validators;

public class CreateArtistVmValidator : AbstractValidator<CreateArtistViewModel>
{
    public CreateArtistVmValidator()
    {
        RuleFor(model => model.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50)
            .DisableTags();

        RuleFor(model => model.About)
            .NotNull()
            .NotEmpty()
            .MaximumLength(500)
            .DisableTags();

        RuleFor(model => model.Photo)
            .NotNull()
            .MaximumDifferenceSidesInPercent(10)
            .MaxSize(ByteSize.FromMegaBytes(1));
    }
}