using SixLabors.ImageSharp;

namespace LiWiMus.Core.Interfaces;

public interface IImageService
{
    Task SavePictureAsync(Image image, string pathToFile);
}