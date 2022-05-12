#region

using LiWiMus.Core.Settings;
using LiWiMus.Web.Shared.Services.Interfaces;

#endregion

namespace LiWiMus.Web.Shared.Extensions;

public static class FormFileSaverExtensions
{
    public static Task<string> SaveWithRandomNameAsync(this IFormFileSaver formFileSaver, ImageFormFile picture)
    {
        return formFileSaver.SaveWithRandomNameAsync(picture, DataType.Picture);
    }
    
    public static Task<string> SaveTrackWithRandomNameAsync(this IFormFileSaver formFileSaver, ImageFormFile picture)
    {
        return formFileSaver.SaveWithRandomNameAsync(picture, DataType.Music);
    }
}