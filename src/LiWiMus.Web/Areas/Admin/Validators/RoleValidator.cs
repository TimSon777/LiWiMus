using FluentValidation;
using LiWiMus.Core.Constants;
using LiWiMus.Web.Areas.Admin.ViewModels;

namespace LiWiMus.Web.Areas.Admin.Validators;

public class RoleValidator : AbstractValidator<RoleViewModel>
{
    public RoleValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20)
            .Matches(RegularExpressions.DisableTags);
        RuleFor(model => model.Description)
            .NotEmpty()
            .MaximumLength(100)
            .Matches(RegularExpressions.DisableTags);
    }
}