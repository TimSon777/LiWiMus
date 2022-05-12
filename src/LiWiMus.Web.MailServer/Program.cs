using System.Reflection;
using FluentValidation.AspNetCore;
using LiWiMus.Web.MailServer.Core.Interfaces;
using LiWiMus.Web.MailServer.Core.Settings;
using LiWiMus.Web.MailServer.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

services.AddControllersWithViews();
services.AddRazorPages();
services.AddEndpointsApiExplorer();

services.Configure<MailSettings>(configuration.GetSection(MailSettings.ConfigName));
services.AddHttpContextAccessor();
services.AddTransient<IRazorViewRenderer, RazorViewRenderer>();
services.AddTransient<IMailRequestService, MailRequestService>();
services.AddTransient<IMailService, MailService>();

services.AddFluentValidation(options =>
{
    options.LocalizationEnabled = false;
    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Run();