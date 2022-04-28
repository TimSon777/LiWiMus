using Ardalis.Specification;
using LiWiMus.SharedKernel.Extensions;

namespace LiWiMus.Core.Users.Specifications;

public sealed class UserWithIncludes : Specification<User>, ISingleResultSpecification
{
    public UserWithIncludes(User user, params string[] includes)
    {
        Query.Where(u => u == user);
        includes.Foreach(include => Query.Include(include));
    }
}