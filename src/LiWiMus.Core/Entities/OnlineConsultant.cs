namespace LiWiMus.Core.Entities;

public class OnlineConsultant : BaseEntity
{
    public User Consultant { get; set; } = default!;

    public string ConnectionId { get; set; } = default!;
    
    public List<Chat> Chats { get; set; } = new();

}