namespace LiWiMus.Web.API.Tracks;

public class Dto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly PublishedAt { get; set; }
    public string FileLocation { get; set; } = null!;
    public double Duration { get; set; }
    public int AlbumId { get; set; }
}