using System.ComponentModel.DataAnnotations.Schema;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Core.UserClaims;

public class UserClaim : IdentityUserClaim<int>, IAggregateRoot
{
    public Users.User User { get; set; } = null!;
    public DateTime GrantedAt { get; set; }
    public DateTime ActiveUntil { get; set; }

    [NotMapped]
    public bool IsActive => ActiveUntil > DateTime.UtcNow;
}