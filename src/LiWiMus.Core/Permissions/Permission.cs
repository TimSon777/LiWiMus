using LiWiMus.Core.Plans;

namespace LiWiMus.Core.Permissions;

public class Permission : BaseEntity
{
    public string Subject { get; set; } = null!;
    public string Action { get; set; } = null!;

    public List<Plan> Plans { get; set; } = new();
}