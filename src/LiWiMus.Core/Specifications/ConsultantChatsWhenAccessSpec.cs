using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public sealed class ConsultantChatsWhenAccessSpec : Specification<Chat>
{
    public ConsultantChatsWhenAccessSpec(User consultant)
    {
        Query.Where(chat => chat.Consultant == consultant);
    }
}