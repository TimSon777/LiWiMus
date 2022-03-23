using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Core.Entities;

public class UserRole : IdentityUserRole<int>
{
    public User User { get; set; }
    public Role Role { get; set; }
    public DateTime GrantedAt { get; set; }
    public DateTime ActiveUntil { get; set; }
}