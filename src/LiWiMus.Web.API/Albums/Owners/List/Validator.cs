using FluentValidation;

namespace LiWiMus.Web.API.Albums.Owners.List;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.AlbumId)
            .NotEmpty();

        RuleFor(request => request.ItemsPerPage)
            .GreaterThan(0);

        RuleFor(request => request.Page)
            .GreaterThan(0);
    }
}