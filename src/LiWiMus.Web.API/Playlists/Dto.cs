using LiWiMus.SharedKernel;

namespace LiWiMus.Web.API.Playlists;

public class Dto : BaseDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;

    public int TracksCount { get; set; }
    public int ListenersCount { get; set; }

    public string Name { get; set; } = null!;
    public bool IsPublic { get; set; }
    public string? PhotoLocation { get; set; }
}