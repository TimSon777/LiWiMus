using SixLabors.ImageSharp;

namespace LiWiMus.Web.Extensions
{
    public static class FormFileExtensions
    {
        public static bool TryParseToImage(this IFormFile file, out Image? image)
        {
            try
            {
                image = Image.Load(file.OpenReadStream());
                return true;
            }
            catch (Exception)
            {
                image = default;
                return false;
            }
        }

        public static async Task<string> SaveWithRandomNameAsync(this IFormFile file, string directory)
        {
            var newFileName = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(file.FileName));
            var newPath = Path.Combine(directory, newFileName);

            var fileInfo = new FileInfo(newPath);
            fileInfo.Directory?.Create();

            var stream = fileInfo.OpenWrite();

            await file.CopyToAsync(stream);
            return newPath;
        }
    }
}