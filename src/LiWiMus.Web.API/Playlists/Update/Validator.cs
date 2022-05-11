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
            .NotEmpty();

        RuleFor(request => request.Name)
            .MaximumLength(100);

        RuleFor(request => request.Photo!)
            .SidesPercentageDifferenceMustBeLessThan(10)
            .MustWeightLessThan(ByteSize.FromMegaBytes(1))
            .When(request => request.Photo is not null);
    }
}