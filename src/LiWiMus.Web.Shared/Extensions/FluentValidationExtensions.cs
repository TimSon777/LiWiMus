#region

using Ardalis.GuardClauses;
using ByteSizeLib;
using FluentValidation;
using FluentValidation.Results;
using LiWiMus.Core.Constants;
using Microsoft.AspNetCore.Http;

#endregion

namespace LiWiMus.Web.Shared.Extensions;

public static class FluentValidationExtensions
{
    public static IDictionary<string, string[]> ToDictionary(this ValidationResult validationResult)
    {
        return validationResult.Errors
                               .GroupBy(x => x.PropertyName)
                               .ToDictionary(
                                   g => g.Key,
                                   g => g.Select(x => x.ErrorMessage).ToArray()
                               );
    }

    public static IRuleBuilderOptions<T, string> DisableTags<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Matches(RegularExpressions.DisableTags);
    }

    public static IRuleBuilderOptions<T, ImageFormFile> SidesPercentageDifferenceMustBeLessThan<T>(
        this IRuleBuilder<T, ImageFormFile> ruleBuilder, double maxPercentageDifference)
    {
        Guard.Against.OutOfRange(maxPercentageDifference, nameof(maxPercentageDifference), 0, 100);
        
        double PercentageDifference(double num1, double num2)
        {
            var absoluteDifference = Math.Abs(num1 - num2);
            var average = (num1 + num2) / 2;
            return absoluteDifference / average * 100;
        }

        return ruleBuilder
               .Must(imageInfo =>
               {
                   if (imageInfo is null)
                   {
                       return true;
                   }
                   
                   return PercentageDifference(imageInfo.Width, imageInfo.Height) <= maxPercentageDifference;
               })
               .WithMessage($"Image width and height must differ by no more than {maxPercentageDifference} percent.");
    }

    public static IRuleBuilderOptions<T, IFormFile> MustWeightLessThan<T>(
        this IRuleBuilder<T, IFormFile> ruleBuilder, ByteSize byteSize)
    {
        return ruleBuilder.Must(file =>
            {
                if (file is null)
                {
                    return true;
                }
                
                return file.Length <= byteSize.Bytes;
            })
                          .WithMessage($"The file must weigh less than {byteSize.ToString()}.");
    }
}