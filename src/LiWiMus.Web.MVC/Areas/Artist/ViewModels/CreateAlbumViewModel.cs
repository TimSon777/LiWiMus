#region

#endregion

namespace LiWiMus.Web.MVC.Areas.Artist.ViewModels;

public class CreateAlbumViewModel
{
    public string Title { get; set; } = null!;
    public string CoverLocation { get; set; } = null!;
    public DateOnly PublishedAt { get; set; }
}