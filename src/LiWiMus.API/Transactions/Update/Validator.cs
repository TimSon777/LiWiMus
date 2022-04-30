using FluentValidation;

namespace LiWiMus.API.Transactions.Update;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.Id)
            .NotEmpty();

        RuleFor(request => request.Description)
            .NotEmpty()
            .MaximumLength(100);
    }
}