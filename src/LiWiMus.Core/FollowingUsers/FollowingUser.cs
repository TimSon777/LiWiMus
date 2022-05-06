namespace LiWiMus.Core.FollowingUsers;

public class FollowingUser : BaseEntity
{
    public virtual User Follower { get; set; } = null!;
    public virtual User Following { get; set; } = null!;

    public int FollowerId { get; set; }
    public int FollowingId { get; set; }
}