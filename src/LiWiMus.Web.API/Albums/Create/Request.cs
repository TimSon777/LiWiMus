using LiWiMus.Web.Shared;

namespace LiWiMus.Web.API.Albums.Create;

public class Request
{
    public string Title { get; set; } = null!;
    public DateOnly PublishedAt { get; set; }
    public ImageFormFile Cover { get; set; } = null!;
    public ICollection<int> ArtistIds { get; set; } = null!;
}