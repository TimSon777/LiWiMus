#region

#endregion

namespace LiWiMus.Web.MVC.Areas.Artist.ViewModels;

public class UpdateAlbumViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? CoverLocation { get; set; }
    public DateOnly PublishedAt { get; set; }

    public int[]? ArtistsIds { get; set; }
}