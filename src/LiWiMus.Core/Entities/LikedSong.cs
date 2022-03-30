namespace LiWiMus.Core.Entities;

public class LikedSong : BaseEntity
{
    public User User { get; set; } = null!;
    public Track Track { get; set; } = null!;
}