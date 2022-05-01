using LiWiMus.Core.Interfaces;
using LiWiMus.SharedKernel.Exceptions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using File = LiWiMus.Core.Files.File;

namespace LiWiMus.Infrastructure.Services;

public class ImageService : IImageService
{
    public async Task SavePictureAsync(Image image, string pathToFile)
    {
        ExceptionHelper.ThrowWhenArgumentNull(image, nameof(image));
        ExceptionHelper.ThrowWhenArgumentNull(pathToFile, nameof(pathToFile));
        await image.SaveAsync(pathToFile);
    }

    public async Task<File> SaveImageAsync(Image image, IImageFormat format)
    {
        throw new NotImplementedException();
    }
}