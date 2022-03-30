using System.ComponentModel.DataAnnotations.Schema;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Core.Entities;

public class UserRole : IdentityUserRole<int>, IAggregateRoot
{
    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
    public DateTime GrantedAt { get; set; }
    public DateTime ActiveUntil { get; set; }

    [NotMapped]
    public bool IsActive => ActiveUntil > DateTime.UtcNow;
}