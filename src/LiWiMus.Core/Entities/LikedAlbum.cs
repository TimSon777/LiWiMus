namespace LiWiMus.Core.Entities;

public class LikedAlbum : BaseEntity
{
    public User User { get; set; } = null!;
    public Album Album { get; set; } = null!;
}