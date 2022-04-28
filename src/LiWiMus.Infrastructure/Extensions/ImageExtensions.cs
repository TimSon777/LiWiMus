using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;

namespace LiWiMus.Infrastructure.Extensions;

public static class ImageExtensions
{
    public static async Task<string> SaveWithRandomNameAsync(this Image image, string directory, string extension)
    {
        var photoFileName = Path.ChangeExtension(Path.GetRandomFileName(), extension);
        var photoPath = Path.Combine(directory, photoFileName);

        var format = image.GetConfiguration().ImageFormatsManager.FindFormatByFileExtension(extension);
        var encoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(format);
        await image.SaveAsync(photoPath, encoder);
        return photoPath;
    }
}