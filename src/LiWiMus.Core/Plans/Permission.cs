namespace LiWiMus.Core.Plans;

public class Permission : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public List<Plan> Plans { get; set; } = new();
}