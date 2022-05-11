using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.Albums.Update;

public class Request : HasId
{
    public string Title { get; set; } = null!;
    public DateTime? PublishedAt { get; set; }
    public string CoverLocation { get; set; } = null!;
}