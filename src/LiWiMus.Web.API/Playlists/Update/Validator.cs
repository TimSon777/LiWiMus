using ByteSizeLib;
using FluentValidation;

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

        RuleFor(request => request.PhotoInfo)
            .NotEmpty();
    }
}