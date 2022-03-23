using FluentValidation;
using LiWiMus.Core.Constants;

namespace LiWiMus.Web.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> DisableTags<T>(
        this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.Matches(RegularExpressions.DisableTags);
}