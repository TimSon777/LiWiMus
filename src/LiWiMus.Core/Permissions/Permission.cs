using System.Security.Claims;
using LiWiMus.Core.Plans;
using LiWiMus.SharedKernel.Interfaces;

namespace LiWiMus.Core.Permissions;

public class Permission : IAggregateRoot
{
    public const string ClaimType = "permission";

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<Plan> Plans { get; set; } = null!;

    public Permission(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public Claim ToClaim()
    {
        return new Claim(ClaimType, Name);
    }
}