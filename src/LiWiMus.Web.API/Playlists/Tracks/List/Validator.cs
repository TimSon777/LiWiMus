using FluentValidation;

namespace LiWiMus.Web.API.Playlists.Tracks.List;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.PlaylistId)
            .NotEmpty();

        RuleFor(request => request.ItemsPerPage)
            .GreaterThan(0);

        RuleFor(request => request.Page)
            .GreaterThan(0);
    }
}