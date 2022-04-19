namespace LiWiMus.Core.Plans;

public class Plan : BaseEntity
{
    public string Title { get; set; } = null!;
    public decimal PricePerMonth { get; set; }

    public List<Permission> Permissions { get; set; } = new();
    public List<UserPlan> UserPlans { get; set; } = new();
}