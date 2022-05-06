using LiWiMus.Core.Artists;

namespace LiWiMus.Core.LikedArtists;

public class LikedArtist : BaseEntity
{
    public virtual User User { get; set; } = null!;
    public virtual Artist Artist { get; set; } = null!;
}