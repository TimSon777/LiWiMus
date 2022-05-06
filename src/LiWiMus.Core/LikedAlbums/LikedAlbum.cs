using LiWiMus.Core.Albums;

namespace LiWiMus.Core.LikedAlbums;

public class LikedAlbum : BaseEntity
{
    public virtual User User { get; set; } = null!;
    public virtual Album Album { get; set; } = null!;
}