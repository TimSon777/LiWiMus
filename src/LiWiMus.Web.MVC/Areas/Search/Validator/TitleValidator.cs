using FluentValidation;
using LiWiMus.Web.MVC.Areas.Search.ViewModels;

namespace LiWiMus.Web.MVC.Areas.Search.Validator;

// ReSharper disable once UnusedType.Global
public class TitleValidator : AbstractValidator<SearchViewModel>
{
    public TitleValidator()
    {
        RuleFor(tvm => tvm.Title)
            .Length(0, 50);
    }
}