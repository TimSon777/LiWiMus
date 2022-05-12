namespace LiWiMus.Web.MailServer.Core.Models;

public class MailRequest
{
    public string ToEmail { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
}