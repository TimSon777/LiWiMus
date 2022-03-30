namespace LiWiMus.Core.Entities;

public class Chat : BaseEntity
{
    public User User { get; set; } = null!;
    public User Consultant { get; set; } = null!;
    public ChatStatus Status { get; set; }

    public List<Message> Messages { get; set; } = new();
}