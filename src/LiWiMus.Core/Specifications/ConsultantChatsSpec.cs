using Ardalis.Specification;
using LiWiMus.Core.Entities;

namespace LiWiMus.Core.Specifications;

public sealed class ConsultantChatsSpec : Specification<Chat>
{
    public ConsultantChatsSpec(OnlineConsultant consultant, ChatStatus status = ChatStatus.Opened)
    {
        Query.Where(chat => chat.Consultant == consultant && chat.Status == status);
    }    
}