using FluentValidation;
using LiWiMus.Core.Users;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Web.API.Transactions.Create;

public class Validator : AbstractValidator<Request>
{
    public Validator(IRepository<User> userRepository)
    {
        RuleFor(request => request.UserId)
            .NotEmpty();

        RuleFor(request => request.Amount)
            .NotEmpty();

        RuleFor(request => request.Description)
            .NotEmpty()
            .MaximumLength(100);
    }
}