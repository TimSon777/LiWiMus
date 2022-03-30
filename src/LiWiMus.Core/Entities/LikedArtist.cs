namespace LiWiMus.Core.Entities;

public class LikedArtist : BaseEntity
{
    public User User { get; set; } = null!;
    public Artist Artist { get; set; } = null!;
}