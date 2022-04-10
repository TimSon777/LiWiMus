using Ardalis.Specification;

namespace LiWiMus.Core.Roles.Specifications;

public sealed class RoleByNameSpec : Specification<Role>, ISingleResultSpecification
{
    public RoleByNameSpec(string roleName)
    {
        Query.Where(role => role.Name == roleName);
    }
}