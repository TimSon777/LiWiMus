#region

using LiWiMus.Web.Shared;

#endregion

namespace LiWiMus.Web.API.Playlists.Update;

public class Request : FromFormRequest<Request>
{
    public int Id { get; set; }
    public string? Name { get; set; } = null!;
    public bool? IsPublic { get; set; }
    public ImageFormFile? Photo { get; set; } = null!;
}