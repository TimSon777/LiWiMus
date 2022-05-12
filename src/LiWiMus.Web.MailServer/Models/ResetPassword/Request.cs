namespace LiWiMus.Web.MailServer.Models.ResetPassword;

public class Request
{
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string ResetUrl { get; set; }
}