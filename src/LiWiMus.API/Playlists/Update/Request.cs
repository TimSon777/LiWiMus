namespace LiWiMus.API.Playlists.Update;

public class Request
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsPublic { get; set; }
    public string PhotoBase64 { get; set; } = null!;
}