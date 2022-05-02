#region

using ByteSizeLib;
using FluentValidation;
using LiWiMus.Web.Shared.Extensions;

#endregion

namespace LiWiMus.Web.API.Playlists.Update;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.Id)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(request => request.Photo)
            .NotEmpty()
            .Must(file => file.Length < ByteSize.BytesInMegaByte);

        RuleFor(request => request.Photo)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .SidesPercentageDifferenceMustBeLessThan(10)
            .MustWeightLessThan(ByteSize.FromMegaBytes(1));
    }
}