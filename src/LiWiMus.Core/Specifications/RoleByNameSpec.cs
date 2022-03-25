using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public sealed class RoleByNameSpec : Specification<Role>, ISingleResultSpecification
{
    public RoleByNameSpec(string roleName)
    {
        Query.Where(role => role.Name == roleName);
    }
}