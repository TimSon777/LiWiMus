namespace LiWiMus.Web.API.Tracks.Create;

public class Request
{
    public int AlbumId { get; set; }
    public string Name { get; set; }
    public DateOnly PublishedAt { get; set; }
    public string FileLocation { get; set; } = null!;
    public List<int> GenreIds { get; set; } = new();
    public List<int> OwnerIds { get; set; } = new();
    public double Duration { get; set; }
}