namespace LiWiMus.Core.Artists;

public class UserArtist : BaseEntity
{
    public virtual User User { get; set; } = null!;
    public virtual Artist Artist { get; set; } = null!;

    public int UserId { get; set; }
    public int ArtistId { get; set; }
}