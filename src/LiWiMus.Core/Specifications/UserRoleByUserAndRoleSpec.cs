using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public sealed class UserRoleByUserAndRoleSpec : Specification<UserRole>, ISingleResultSpecification
{
    public UserRoleByUserAndRoleSpec(User user, Role role)
    {
        Query.Where(userRole => userRole.UserId == user.Id && userRole.RoleId == role.Id);
    }
}