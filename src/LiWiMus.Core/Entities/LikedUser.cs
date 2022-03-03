namespace LiWiMus.Core.Entities;

public class LikedUser : BaseEntity
{
    public User User { get; set; }
    public User Liked { get; set; }
}