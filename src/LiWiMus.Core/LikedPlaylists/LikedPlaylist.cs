using LiWiMus.Core.Playlists;

namespace LiWiMus.Core.LikedPlaylists;

public class LikedPlaylist : BaseEntity
{
    public virtual User User { get; set; } = null!;
    public virtual Playlist Playlist { get; set; } = null!;
}