using LiWiMus.Core.Chats;

namespace LiWiMus.Core.Messages;

public class Message : BaseEntity
{
    public virtual Chat Chat { get; set; } = null!;
    public virtual User? Owner { get; set; }
    public string Text { get; set; } = null!;
}