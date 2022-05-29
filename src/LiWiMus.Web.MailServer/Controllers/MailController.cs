using LiWiMus.Web.MailServer.Core.Interfaces;
using LiWiMus.Web.MailServer.Core.Models;
using LiWiMus.Web.MailServer.Models.ConfirmAccount;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    public IActionResult Test()
    {
        return Ok("test");
    }
    
    [HttpPost]
    public async Task ConfirmAccount(Request request)
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
    public async Task ResetPassword(Models.ResetPassword.Request request)
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