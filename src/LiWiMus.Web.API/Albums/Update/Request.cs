using LiWiMus.SharedKernel;
using LiWiMus.Web.Shared;

namespace LiWiMus.Web.API.Albums.Update;

public class Request : HasId
{
    public string Title { get; set; } = null!;
    public DateOnly? PublishedAt { get; set; }
    public ImageFormFile Cover { get; set; } = null!;
}