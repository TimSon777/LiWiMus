using LiWiMus.Web.MailServer.Core.Models;

namespace LiWiMus.Web.MailServer.Core.Interfaces;

public interface IMailRequestService
{
    public Task<MailRequest> CreateConfirmEmailAsync(string userName, string email, string confirmUrl);
    Task<MailRequest> CreateResetPasswordEmailAsync(string userName, string email, string resetUrl);
}