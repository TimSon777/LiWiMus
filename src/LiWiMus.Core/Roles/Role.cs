namespace LiWiMus.Core.Roles;

public class Role : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public virtual ICollection<SystemPermission> Permissions { get; set; } = null!;
    public virtual ICollection<User> Users { get; set; } = null!;
}