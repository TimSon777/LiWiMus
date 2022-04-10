using System.Security.Claims;
using Ardalis.Specification;

namespace LiWiMus.Core.UserClaims.Specifications;

public sealed class UserClaimByUserAndClaimSpec : Specification<UserClaim>, ISingleResultSpecification
{
    public UserClaimByUserAndClaimSpec(User user, Claim claim)
    {
        Query.Where(userClaim => userClaim.UserId == user.Id &&
                                 userClaim.ClaimType == claim.Type &&
                                 userClaim.ClaimValue == claim.Value);
    }
}