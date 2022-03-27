using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public sealed class UsersByActiveRoleSpec : Specification<UserRole, User>
{
    public UsersByActiveRoleSpec(Role role)
    {
        Query.Where(userRole => userRole.RoleId == role.Id && userRole.ActiveUntil < DateTime.UtcNow);
        Query.Select(userRole => userRole.User);
    }
}