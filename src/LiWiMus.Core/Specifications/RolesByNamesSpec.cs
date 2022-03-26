using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public sealed class RolesByNamesSpec : Specification<Role>
{
    public RolesByNamesSpec(IEnumerable<string> names)
    {
        Query.Where(role => names.Contains(role.Name));
    }
}