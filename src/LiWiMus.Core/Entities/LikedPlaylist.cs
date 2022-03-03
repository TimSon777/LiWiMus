namespace LiWiMus.Core.Entities;

public class LikedPlaylist : BaseEntity
{
    public User User { get; set; }
    public Playlist Playlist { get; set; }
}