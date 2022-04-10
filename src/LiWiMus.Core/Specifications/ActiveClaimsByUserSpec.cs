using System.Security.Claims;
using Ardalis.Specification;
using LiWiMus.Core.UserClaims;

namespace LiWiMus.Core.Specifications;

public sealed class ActiveClaimsByUserSpec : Specification<UserClaim, Claim>
{
    public ActiveClaimsByUserSpec(User user)
    {
        Query.Where(userClaim => userClaim.UserId == user.Id && userClaim.ActiveUntil > DateTime.UtcNow);
        Query.Select(claim => new Claim(claim.ClaimType, claim.ClaimValue));
    }
}