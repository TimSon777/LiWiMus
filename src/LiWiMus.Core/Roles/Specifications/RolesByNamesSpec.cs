using Ardalis.Specification;

namespace LiWiMus.Core.Roles.Specifications;

public sealed class RolesByNamesSpec : Specification<Role>
{
    public RolesByNamesSpec(IEnumerable<string> names)
    {
        Query.Where(role => names.Contains(role.Name));
    }
}