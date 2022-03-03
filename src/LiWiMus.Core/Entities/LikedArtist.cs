namespace LiWiMus.Core.Entities;

public class LikedArtist : BaseEntity
{
    public User User { get; set; }
    public Artist Artist { get; set; }
}