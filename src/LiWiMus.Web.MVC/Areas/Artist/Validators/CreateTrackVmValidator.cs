﻿#region

using ByteSizeLib;
using FluentValidation;
using LiWiMus.Web.MVC.Areas.Artist.ViewModels;
using LiWiMus.Web.Shared.Extensions;

#endregion

namespace LiWiMus.Web.MVC.Areas.Artist.Validators;

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

        RuleFor(model => model.File)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .MustWeightLessThan(ByteSize.FromMegaBytes(20));
    }
}