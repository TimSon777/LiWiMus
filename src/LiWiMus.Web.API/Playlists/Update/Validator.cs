#region

using FluentValidation;

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
    }
}