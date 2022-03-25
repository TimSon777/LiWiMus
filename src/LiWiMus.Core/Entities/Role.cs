using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Core.Entities;

public class Role : IdentityRole<int>, IAggregateRoot
{
    public Role(string name, string description, bool isPublic = false, decimal? pricePerMonth = null) : base(name)
    {
        Description = description;
        PricePerMonth = pricePerMonth;
        IsPublic = isPublic;
        // ReSharper disable once VirtualMemberCallInConstructor
        NormalizedName = name.ToUpperInvariant();
    }
    public string Description { get; set; }
    public decimal? PricePerMonth { get; set; }
    public bool IsPublic { get; set; }

    public List<UserRole> UserRoles { get; set; } = new();
}