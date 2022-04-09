using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public sealed class OpenedChatByUserSpec : Specification<Chat>, ISingleResultSpecification
{
    public OpenedChatByUserSpec(User user)
    {
        Query.Where(chat => chat.User.Id == user.Id && chat.Status == ChatStatus.Opened);
    }
}