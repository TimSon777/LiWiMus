using ByteSizeLib;
using FluentValidation;
using LiWiMus.Core.Constants;
using LiWiMus.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace LiWiMus.Web.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> DisableTags<T>(
        this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.Matches(RegularExpressions.DisableTags);

    public static IRuleBuilderOptions<T, ImageInfo> MaximumDifferenceSidesInPercent<T>(
        this IRuleBuilder<T, ImageInfo> ruleBuilder, int maximumAspectRatioPercentage)
    {
        if (maximumAspectRatioPercentage is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumAspectRatioPercentage));
        }

        return ruleBuilder.Must(imageInfo =>
                          {
                              if (imageInfo is null)
                              {
                                  return true;
                              }
                              var image = imageInfo.Image;
                              var width = image.Width;
                              var height = image.Height;
                              return Math.Abs(width - height) / (double) Math.Max(width, height) * 100 <=
                                     maximumAspectRatioPercentage;
                          })
                          .WithMessage(
                              $"Image width and height must differ by no more than {maximumAspectRatioPercentage} percent");
    }

    public static IRuleBuilderOptions<T, ImageInfo?> MaxSize<T>(
        this IRuleBuilder<T, ImageInfo?> ruleBuilder, ByteSize byteSize)
    {
        return ruleBuilder.Must(image =>
                          {
                              if (image is null)
                              {
                                  return true;
                              }

                              return image.ByteSize <= byteSize;
                          })
                          .WithMessage($"The image must weigh less than {byteSize.ToString()}");
    }
}