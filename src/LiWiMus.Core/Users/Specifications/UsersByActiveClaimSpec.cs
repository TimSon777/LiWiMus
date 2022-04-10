using System.Security.Claims;
using Ardalis.Specification;
using LiWiMus.Core.UserClaims;

namespace LiWiMus.Core.Users.Specifications;

public sealed class UsersByActiveClaimSpec : Specification<UserClaim, User>
{
    public UsersByActiveClaimSpec(Claim claim)
    {
        Query.Where(userClaim => userClaim.ClaimType == claim.Type && userClaim.ClaimValue == claim.Value &&
                                 userClaim.ActiveUntil < DateTime.UtcNow);
        Query.Select(userClaim => userClaim.User);
    }
}