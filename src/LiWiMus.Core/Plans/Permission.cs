namespace LiWiMus.Core.Plans;

public class Permission : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public virtual ICollection<Plan> Plans { get; set; } = null!;
}