using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Models;
using LiWiMus.Core.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace LiWiMus.Infrastructure.Services;

public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;
    private readonly IMailRequestService _mailRequestService;

    public MailService(IOptions<MailSettings> mailSettings, IMailRequestService mailRequestService)
    {
        _mailRequestService = mailRequestService;
        _mailSettings = mailSettings.Value;
    }

    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
        email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
        email.Subject = mailRequest.Subject;
        var builder = new BodyBuilder
        {
            HtmlBody = mailRequest.Body
        };
        email.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port);
        await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }

    public async Task SendConfirmEmailAsync(string userName, string email, string confirmUrl)
    {
        var mailRequest = await _mailRequestService.CreateConfirmEmailAsync(userName, email, confirmUrl);
        await SendEmailAsync(mailRequest);
    }
}