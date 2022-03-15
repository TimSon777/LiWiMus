using LiWiMus.Core.Models;

namespace LiWiMus.Core.Interfaces;

public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);
    Task SendConfirmEmailAsync(string userName, string email, string confirmUrl);
}