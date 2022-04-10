using LiWiMus.Core.Chats.Enums;

namespace LiWiMus.Web.Areas.User.ViewModels;

public class ChatViewModel
{
    public UserChatViewModel User { get; set; }
    public UserChatViewModel Consultant { get; set; }
    public string UserConnectionId { get; set; }
    
    public ChatStatus Status { get; set; }

    public List<MessageViewModel> Messages { get; set; }
}