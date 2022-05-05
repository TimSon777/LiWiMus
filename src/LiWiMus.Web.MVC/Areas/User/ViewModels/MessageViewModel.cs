namespace LiWiMus.Web.MVC.Areas.User.ViewModels;

public class MessageViewModel
{
    public UserChatViewModel Owner { get; set; }
    public string Text { get; set; }
}