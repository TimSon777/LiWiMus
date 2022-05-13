using System.Reflection;
using FluentValidation.AspNetCore;
using LiWiMus.Web.MailServer.Core.Interfaces;
using LiWiMus.Web.MailServer.Core.Settings;
using LiWiMus.Web.MailServer.Infrastructure.Services;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

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

services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = builder.Environment.ApplicationName });
    o.CustomSchemaIds(type => type.ToString());
    o.AddFluentValidationRulesScoped();
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", builder.Environment.ApplicationName);
    c.RoutePrefix = "api/swagger";
});

app.MapControllers();

app.Run();