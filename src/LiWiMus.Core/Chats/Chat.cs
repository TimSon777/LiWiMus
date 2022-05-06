using LiWiMus.Core.Chats.Enums;
using LiWiMus.Core.Messages;

namespace LiWiMus.Core.Chats;

public class Chat : BaseEntity
{
    public virtual User User { get; set; } = null!;

    public virtual ChatStatus Status { get; set; } = ChatStatus.Opened;
    
    public string UserConnectionId { get; set; } = "";

    public string? ConsultantConnectionId { get; set; } = "";

    public virtual ICollection<Message> Messages { get; set; } = null!;
}