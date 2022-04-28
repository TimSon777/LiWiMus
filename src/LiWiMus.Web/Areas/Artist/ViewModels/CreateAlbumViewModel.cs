using LiWiMus.Core.Models;

namespace LiWiMus.Web.Areas.Artist.ViewModels;

public class CreateAlbumViewModel
{
    public string Title { get; set; } = null!;
    public ImageInfo Cover { get; set; } = null!;
    public DateOnly PublishedAt { get; set; }
}