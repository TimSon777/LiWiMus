using Ardalis.Specification;
using LiWiMus.Core.Roles;
using LiWiMus.Core.UserRoles;

namespace LiWiMus.Core.Users.Specifications;

public sealed class UsersByActiveRoleSpec : Specification<UserRole, User>
{
    public UsersByActiveRoleSpec(Role role)
    {
        Query.Where(userRole => userRole.RoleId == role.Id && userRole.ActiveUntil < DateTime.UtcNow);
        Query.Select(userRole => userRole.User);
    }
}