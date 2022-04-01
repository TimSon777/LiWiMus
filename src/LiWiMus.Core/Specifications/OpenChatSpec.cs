using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public sealed class OpenChatSpec : Specification<Chat>, ISingleResultSpecification
{
    public OpenChatSpec(User user)
    {
        Query.Where(chat => chat.User == user && chat.Status == ChatStatus.Opened);
    }
}