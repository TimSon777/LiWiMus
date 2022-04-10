using LiWiMus.Core.UserRoles;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Core.Roles;

public class Role : IdentityRole<int>, IAggregateRoot
{
    public Role(string name, string description, TimeSpan defaultTimeout, bool isPublic = false, decimal? pricePerMonth = null) : base(name)
    {
        Description = description;
        DefaultTimeout = defaultTimeout;
        PricePerMonth = pricePerMonth;
        IsPublic = isPublic;
        // ReSharper disable once VirtualMemberCallInConstructor
        NormalizedName = name.ToUpperInvariant();
    }
    public string Description { get; set; }
    public decimal? PricePerMonth { get; set; }
    public bool IsPublic { get; set; }
    public TimeSpan DefaultTimeout { get; set; }

    public List<UserRole> UserRoles { get; set; } = new();
}