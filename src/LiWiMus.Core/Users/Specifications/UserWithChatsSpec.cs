using Ardalis.Specification;

namespace LiWiMus.Core.Users.Specifications;

public sealed class UserWithChatsSpec : Specification<User>, ISingleResultSpecification
{
    public UserWithChatsSpec(User user)
    {
        Query
            .Where(u => u == user)
            .Include(u => u.UserChats)
            .ThenInclude(c => c.Messages);
    }
}