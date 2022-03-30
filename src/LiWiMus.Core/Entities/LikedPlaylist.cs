namespace LiWiMus.Core.Entities;

public class LikedPlaylist : BaseEntity
{
    public User User { get; set; } = null!;
    public Playlist Playlist { get; set; } = null!;
}