#region

using ByteSizeLib;
using FluentValidation;
using LiWiMus.Web.MVC.Areas.Artist.ViewModels;
using LiWiMus.Web.Shared.Extensions;

#endregion

namespace LiWiMus.Web.MVC.Areas.Artist.Validators;

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

        RuleFor(model => model.File!)
            .MustWeightLessThan(ByteSize.FromMegaBytes(20))
            .When(model => model.File is not null);

        RuleFor(model => model.ArtistsIds)
            .NotEmpty()
            .When(model => model.ArtistsIds is not null);

        RuleFor(model => model.GenresIds)
            .NotEmpty()
            .When(model => model.GenresIds is not null);
    }
}