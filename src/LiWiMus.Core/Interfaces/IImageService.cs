using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using File = LiWiMus.Core.Files.File;

namespace LiWiMus.Core.Interfaces;

public interface IImageService
{
    Task SavePictureAsync(Image image, string pathToFile);
    Task<File> SaveImageAsync(Image image, IImageFormat format);
}