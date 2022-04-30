using FluentValidation;

namespace LiWiMus.API.Playlists.Update;

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

        RuleFor(request => request.PhotoBase64)
            .NotEmpty();
    }
}