using FluentValidation;
using LiWiMus.Core.Users;
using LiWiMus.Core.Users.Specifications;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Web.API.Transactions.Create;

public class Validator : AbstractValidator<Request>
{
    public Validator(IRepository<User> userRepository)
    {
        RuleFor(request => request.UserId)
            .NotEmpty()
            .MustAsync(async (id, token) => await userRepository.AnyAsync(new UserByIdSpec(id), token))
            .WithMessage("There is no user with this id: {PropertyValue}");

        RuleFor(request => request.Amount)
            .NotEmpty();

        RuleFor(request => request.Description)
            .NotEmpty()
            .MaximumLength(100);
    }
}