using System.ComponentModel.DataAnnotations.Schema;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LiWiMus.Core.Entities;

public class UserClaim : IdentityUserClaim<int>, IAggregateRoot
{
    public DateTime GrantedAt { get; set; }
    public DateTime ActiveUntil { get; set; }

    [NotMapped]
    public bool IsActive => ActiveUntil > DateTime.UtcNow;
}