using LiWiMus.Core.Interfaces;

namespace LiWiMus.Core.Models;

public class MailRequest
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }

    public static async Task<MailRequest> CreateConfirmEmailAsync(IRazorViewRenderer razorViewRenderer, string userName,
        string email, string confirmUrl)
    {
        var body = await razorViewRenderer.RenderViewAsync(
            "/Views/Emails/ConfirmAccount/ConfirmAccount.cshtml",
            (userName, confirmUrl));

        return new MailRequest
        {
            Body = body,
            Subject = "Подтверждение аккаунта",
            ToEmail = email
        };
    }
}