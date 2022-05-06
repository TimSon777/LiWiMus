using LiWiMus.Core.Tracks;

namespace LiWiMus.Core.LikedSongs;

public class LikedSong : BaseEntity
{
    public virtual User User { get; set; } = null!;
    public virtual Track Track { get; set; } = null!;
}