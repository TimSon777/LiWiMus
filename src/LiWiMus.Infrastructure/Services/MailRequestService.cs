using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Models;

namespace LiWiMus.Infrastructure.Services;

public class MailRequestService : IMailRequestService
{
    private readonly IRazorViewRenderer _razorViewRenderer;

    public MailRequestService(IRazorViewRenderer razorViewRenderer)
    {
        _razorViewRenderer = razorViewRenderer;
    }

    public async Task<MailRequest> CreateConfirmEmailAsync(string userName, string email, string confirmUrl)
    {
        var body = await _razorViewRenderer.RenderViewAsync(
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