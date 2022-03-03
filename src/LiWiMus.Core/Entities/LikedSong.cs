namespace LiWiMus.Core.Entities;

public class LikedSong : BaseEntity
{
    public User User { get; set; }
    public Track Track { get; set; }
}