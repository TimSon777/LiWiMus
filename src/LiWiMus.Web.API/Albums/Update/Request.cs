using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.Albums.Update;

public class Request : HasId
{
    public string? Title { get; set; }
    public DateOnly? PublishedAt { get; set; }
    public string? CoverLocation { get; set; }
}