using LiWiMus.Core.Chats.Enums;
using LiWiMus.Core.Messages;
using LiWiMus.Core.OnlineConsultants;

namespace LiWiMus.Core.Chats;

public class Chat : BaseEntity
{
    public User User { get; set; } = null!;
    public OnlineConsultant Consultant { get; set; } = null!;
    public ChatStatus Status { get; set; } = ChatStatus.Opened;
    public string UserConnectionId { get; set; }

    public List<Message> Messages { get; set; } = new();
}