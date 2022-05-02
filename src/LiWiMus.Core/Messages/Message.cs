using LiWiMus.Core.Chats;
using LiWiMus.Core.Shared.Interfaces;

namespace LiWiMus.Core.Messages;

public class Message : BaseEntity, IResource.WithOwner<User>
{
    public Chat Chat { get; set; } = null!;
    public User Owner { get; set; } = null!;
    public string Text { get; set; } = null!;
}