using FluentValidation;
using LiWiMus.Web.MVC.Areas.Artist.ViewModels;

namespace LiWiMus.Web.MVC.Areas.Artist.Validators;

public class CreateAlbumVmValidator : AbstractValidator<CreateAlbumViewModel>
{
    public CreateAlbumVmValidator()
    {
        RuleFor(model => model.Cover)
            .NotEmpty();
        RuleFor(model => model.Title)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(model => model.PublishedAt)
            .NotEmpty();
    }
}