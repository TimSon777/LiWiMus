using Ardalis.Specification;

namespace LiWiMus.Core.UserRoles.Specifications;

public sealed class UserRoleByUserAndNormalizedRoleNameSpec : Specification<UserRole>, ISingleResultSpecification
{
    public UserRoleByUserAndNormalizedRoleNameSpec(User user, string normalizedRoleName)
    {
        Query.Where(userRole => userRole.UserId == user.Id && userRole.Role.NormalizedName == normalizedRoleName);
    }
}