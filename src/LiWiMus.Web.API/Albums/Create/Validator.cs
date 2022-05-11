﻿using ByteSizeLib;
using FluentValidation;
using LiWiMus.Web.API.Shared;
using LiWiMus.Web.Shared.Extensions;

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
            .WithMessage(ValidationConstants.DateLessThenNow);
        
        RuleFor(r => r.Cover)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .SidesPercentageDifferenceMustBeLessThan(10)
            .MustWeightLessThan(ByteSize.FromMegaBytes(1));
    }
}