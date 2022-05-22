namespace LiWiMus.Core.Plans;

public class Plan : BaseEntity
{
    public string Name { get; set; } = null!;
    public decimal PricePerMonth { get; set; }
    public string Description { get; set; } = null!;

    public virtual ICollection<Permission> Permissions { get; set; } = null!;
    public virtual ICollection<UserPlan> UserPlans { get; set; } = null!;
}