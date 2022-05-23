namespace LiWiMus.Web.API.Albums.Create;

public class Request
{
    public string Title { get; set; } = null!;
    public DateOnly PublishedAt { get; set; }
    public string CoverLocation { get; set; } = null!;
}