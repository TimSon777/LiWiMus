using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public sealed class UserRoleByUserAndNormalizedRoleNameSpec : Specification<UserRole>, ISingleResultSpecification
{
    public UserRoleByUserAndNormalizedRoleNameSpec(User user, string normalizedRoleName)
    {
        Query.Where(userRole => userRole.UserId == user.Id && userRole.Role.NormalizedName == normalizedRoleName);
    }
}