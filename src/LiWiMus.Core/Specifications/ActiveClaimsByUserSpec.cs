using System.Security.Claims;
using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public sealed class ActiveClaimsByUserSpec : Specification<UserClaim, Claim>
{
    public ActiveClaimsByUserSpec(User user)
    {
        Query.Where(claim => claim.UserId == user.Id && claim.ActiveUntil > DateTime.UtcNow);
        Query.Select(claim => new Claim(claim.ClaimType, claim.ClaimValue));
    }
}