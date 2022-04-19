using System.Security.Claims;
using LiWiMus.Core.Plans;

namespace LiWiMus.Core.Permissions;

public class Permission
{
    public const string ClaimType = "Permission";

    public int Id { get; set; }
    public string Name { get; set; }

    public List<Plan> Plans { get; set; } = new();

    public Permission(string name)
    {
        Name = name;
    }

    public Claim ToClaim()
    {
        return new Claim(ClaimType, Name);
    }
}