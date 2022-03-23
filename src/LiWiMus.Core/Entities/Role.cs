using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Core.Entities;

public sealed class Role : IdentityRole<int>
{
    public Role(string name, string description, bool isPublic = false, decimal? pricePerMonth = null) : base(name)
    {
        Description = description;
        PricePerMonth = pricePerMonth;
        IsPublic = isPublic;
        NormalizedName = name.ToUpperInvariant();
    }
    public string Description { get; set; }
    public decimal? PricePerMonth { get; set; }
    public bool IsPublic { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }
}