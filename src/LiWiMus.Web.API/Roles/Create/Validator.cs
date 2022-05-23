using FluentValidation;
using LiWiMus.Core.Roles;
using LiWiMus.Core.Roles.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.Shared.Extensions;

namespace LiWiMus.Web.API.Roles.Create;

public class Validator : AbstractValidator<Request>
{
    public Validator(IRepository<Role> repository)
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(50)
            .DisableTags()
            .MustAsync(async (s, token) => !await repository.AnyAsync(new RoleByNameSpec(s), token))
            .WithMessage("Role with this name already exists");

        RuleFor(request => request.Description)
            .NotEmpty()
            .MaximumLength(500)
            .DisableTags();
    }
}