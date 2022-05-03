using Ardalis.Specification;
using LiWiMus.SharedKernel.Extensions;

namespace LiWiMus.Core.Users.Specifications;

public sealed class UserWithIncludesByIdSpec : Specification<User>, ISingleResultSpecification
{
    public UserWithIncludesByIdSpec(int id, params string[] includes)
    {
        Query.Where(u => u.Id == id);
        includes.Foreach(include => Query.Include(include));
    }
}