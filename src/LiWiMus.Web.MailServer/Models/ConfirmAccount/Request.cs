namespace LiWiMus.Web.MailServer.Models.ConfirmAccount;

public class Request
{
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string ConfirmUrl { get; set; }
}