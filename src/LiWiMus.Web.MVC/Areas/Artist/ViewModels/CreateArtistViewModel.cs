#region

using LiWiMus.Web.Shared;

#endregion

namespace LiWiMus.Web.MVC.Areas.Artist.ViewModels;

public class CreateArtistViewModel
{
    public string Name { get; set; } = null!;
    public string About { get; set; } = null!;

    public ImageFormFile Photo { get; set; } = null!;
}