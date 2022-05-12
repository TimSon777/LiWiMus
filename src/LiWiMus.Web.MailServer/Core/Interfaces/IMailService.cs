using LiWiMus.Web.MailServer.Core.Models;

namespace LiWiMus.Web.MailServer.Core.Interfaces;

public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);
    Task SendConfirmEmailAsync(string userName, string email, string confirmUrl);
    Task SendResetPasswordAsync(string userName, string email, string resetUrl);
}