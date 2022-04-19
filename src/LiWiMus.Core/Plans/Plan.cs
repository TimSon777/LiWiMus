using System.Security.Claims;
using LiWiMus.Core.Permissions;

namespace LiWiMus.Core.Plans;

public class Plan : BaseEntity
{
    public const string ClaimType = "Plan";

    public string Name { get; set; }
    public decimal PricePerMonth { get; set; }

    public List<Permission> Permissions { get; set; } = new();
    public List<UserPlan> UserPlans { get; set; } = new();

    public Plan(string name, decimal pricePerMonth)
    {
        Name = name;
        PricePerMonth = pricePerMonth;
    }

    public Claim ToClaim()
    {
        return new Claim(ClaimType, Name);
    }
}