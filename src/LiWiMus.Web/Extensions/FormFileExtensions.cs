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
    }
}