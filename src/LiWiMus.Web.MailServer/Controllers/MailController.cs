using LiWiMus.Web.MailServer.Core.Interfaces;
using LiWiMus.Web.MailServer.Core.Models;
using LiWiMus.Web.MailServer.Models.ConfirmAccount;
using LiWiMus.Web.MailServer.Models.ResetPassword;
using Microsoft.AspNetCore.Mvc;
using Request = LiWiMus.Web.MailServer.Models.ResetPassword.Request;

namespace LiWiMus.Web.MailServer.Controllers;

[ApiController, Route("mail/[action]")]
public class MailController : ControllerBase
{
    private readonly IRazorViewRenderer _razorViewRenderer;
    private readonly IMailService _mailService;

    public MailController(IRazorViewRenderer razorViewRenderer, IMailService mailService)
    {
        _razorViewRenderer = razorViewRenderer;
        _mailService = mailService;
    }
    
    [HttpPost]
    public async Task ConfirmAccount(Models.ConfirmAccount.Request request)
    {
        var body = await _razorViewRenderer.RenderViewAsync("/Views/Emails/Body/ConfirmAccount.cshtml",
            (request.UserName, request.ConfirmUrl));

        var mailRequest = new MailRequest
        {
            Body = body,
            Subject = "Account Confirmation",
            ToEmail = request.UserEmail
        };

        await _mailService.SendEmailAsync(mailRequest);
    }
    
    [HttpPost]
    public async Task ResetPassword(Request request)
    {
        var body = await _razorViewRenderer.RenderViewAsync("/Views/Emails/Body/ResetPassword.cshtml",
            (request.UserName, request.ResetUrl));

        var mailRequest = new MailRequest
        {
            Body = body,
            Subject = "Reset password",
            ToEmail = request.UserEmail
        };

        await _mailService.SendEmailAsync(mailRequest);
    }
}