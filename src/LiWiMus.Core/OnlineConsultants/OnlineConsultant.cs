using LiWiMus.Core.Chats;

namespace LiWiMus.Core.OnlineConsultants;

public class OnlineConsultant : BaseEntity
{
    public virtual User Consultant { get; set; } = default!;

    public string ConnectionId { get; set; } = default!;

    public virtual ICollection<Chat> Chats { get; set; } = null!;
}