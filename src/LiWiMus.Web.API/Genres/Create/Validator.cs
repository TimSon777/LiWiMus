using FluentValidation;
using LiWiMus.Core.Genres;
using LiWiMus.Core.Genres.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.Shared.Extensions;

namespace LiWiMus.Web.API.Genres.Create;

public class Validator : AbstractValidator<Request>
{
    public Validator(IRepository<Genre> repository)
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .Length(2, 50)
            .DisableTags()
            .MustAsync(async (name, token) => !await repository.AnyAsync(new GenreByNameSpec(name), token))
            .WithMessage("Genre with name '{PropertyValue}' already exists");
    }
}