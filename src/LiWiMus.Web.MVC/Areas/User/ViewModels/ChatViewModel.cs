using LiWiMus.Core.Chats.Enums;

namespace LiWiMus.Web.MVC.Areas.User.ViewModels;

public class ChatViewModel
{
    public UserChatViewModel User { get; set; }

    public List<UserChatViewModel> Consultants { get; set; }

    public string UserConnectionId { get; set; }
    public string ConsultantConnectionId { get; set; }
    public ChatStatus Status { get; set; }

    public List<MessageViewModel> Messages { get; set; }
}