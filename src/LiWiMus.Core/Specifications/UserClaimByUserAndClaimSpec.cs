using System.Security.Claims;
using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public sealed class UserClaimByUserAndClaimSpec : Specification<UserClaim>, ISingleResultSpecification
{
    public UserClaimByUserAndClaimSpec(User user, Claim claim)
    {
        Query.Where(userClaim => userClaim.UserId == user.Id &&
                                 userClaim.ClaimType == claim.Type &&
                                 userClaim.ClaimValue == claim.Value);
    }
}