#region

#endregion

namespace LiWiMus.Web.API.Playlists.Update;

public class Request
{
    public int Id { get; set; }
    public string? Name { get; set; } = null!;
    public bool? IsPublic { get; set; }
    public string? PhotoLocation { get; set; } = null!;
}