namespace LiWiMus.Core.Entities;

public class LikedAlbum : BaseEntity
{
    public User User { get; set; }
    public Album Album { get; set; }
}