namespace LiWiMus.Core.Entities;

public class LikedUser : BaseEntity
{
    public User User { get; set; } = null!;
    public User Liked { get; set; } = null!;
}