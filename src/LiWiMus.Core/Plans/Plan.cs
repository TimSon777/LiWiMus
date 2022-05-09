using System.Security.Claims;
using LiWiMus.Core.Permissions;

namespace LiWiMus.Core.Plans;

public class Plan : BaseEntity
{
    public const string ClaimType = "Plan";

    public string Name { get; set; } = null!;
    public decimal PricePerMonth { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = null!;
    public virtual ICollection<UserPlan> UserPlans { get; set; } = null!;

    public Claim ToClaim()
    {
        return new Claim(ClaimType, Name);
    }
}