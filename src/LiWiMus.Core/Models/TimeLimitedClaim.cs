using System.Security.Claims;
using LiWiMus.SharedKernel.Extensions;

namespace LiWiMus.Core.Models;

public class TimeLimitedClaim : Claim
{
    public DateTime GrantedAt { get; set; }
    public DateTime ActiveUntil { get; set; }

    public TimeLimitedClaim(string type, string value) : base(type, value)
    {
        GrantedAt = DateTime.UtcNow;
        ActiveUntil = DateTime.MaxValue;
    }

    public TimeLimitedClaim(string type, string value, DateTime grantedAt, DateTime activeUntil) : base(type, value)
    {
        GrantedAt = grantedAt;
        ActiveUntil = activeUntil;
    }

    public TimeLimitedClaim(string type, string value, DateTime activeUntil) : base(type, value)
    {
        GrantedAt = DateTime.UtcNow;
        ActiveUntil = activeUntil;
    }

    public TimeLimitedClaim(string type, string value, TimeSpan timeout) : base(type, value)
    {
        GrantedAt = DateTime.UtcNow;
        ActiveUntil = DateTime.UtcNow.AddOrMaximize(timeout);
    }
}