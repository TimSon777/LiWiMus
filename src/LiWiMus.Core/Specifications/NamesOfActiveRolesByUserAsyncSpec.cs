using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public sealed class NamesOfActiveRolesByUserAsyncSpec : Specification<Role, string>
{
    public NamesOfActiveRolesByUserAsyncSpec(User user)
    {
        Query.Where(role =>
            role.UserRoles.Any(userRole => userRole.UserId == user.Id && userRole.IsActive));
        Query.Select(role => role.Name);
    }
}