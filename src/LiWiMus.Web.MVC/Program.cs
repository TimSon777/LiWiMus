using System.Reflection;
using EntityFrameworkCore.Triggers;
using FluentValidation.AspNetCore;
using FormHelper;
using LiWiMus.Core.Interfaces.Files;
using LiWiMus.Core.Interfaces.Mail;
using LiWiMus.Core.Settings;
using LiWiMus.Infrastructure.Data.Config;
using LiWiMus.Infrastructure.Identity;
using LiWiMus.Web.MVC.Hubs.SupportChat;
using LiWiMus.Web.MVC.Models;
using LiWiMus.Web.Shared.Configuration;
using LiWiMus.Web.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Refit;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Logging.AddSimpleConsole();

configuration.AddEnvironmentVariables();

var services = builder.Services;

services.AddSharedServices();
services.AddDbContext(configuration.GetConnectionString("DefaultConnection"));
services.AddCoreServices();
services.AddTriggers();
var section = configuration.GetRequiredSection(nameof(PullUrls));
var pullUrls = section.Get<PullUrls>();
services.Configure<PullUrls>(section);
services
    .AddRefitClient<IMailService>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(pullUrls.MailServer);
    });
services
    .AddRefitClient<IFileService>()
    .ConfigureHttpClient(c => { c.BaseAddress = new Uri(pullUrls.FileServer); });

TriggersConfiguration.ConfigureTriggers();

services.AddHttpClient();
services.AddIdentity(builder.Environment);
services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/Account/Login";
    options.AccessDeniedPath = "/User/Account/Denied";
});

services.AddControllersWithViews(options => options.UseDateOnlyTimeOnlyStringConverters())
        .AddFluentValidation(fv =>
        {
            fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            fv.LocalizationEnabled = false;
        })
        .AddFormHelper(options => { options.EmbeddedFiles = true; });

services.AddAutoMapper(Assembly.GetExecutingAssembly());

services.AddAuthentication()
        .AddGoogle(options =>
        {
            options.ClientId = configuration.GetValue<string>("GoogleAuthSettings:ClientId");
            options.ClientSecret = configuration.GetValue<string>("GoogleAuthSettings:ClientSecret");
        });

services.AddAuthorization(options =>
{
    options.AddPermissionPolicies();
    
    options.AddPolicy("SameAuthorPolicy",
        policyBuilder => policyBuilder.AddRequirements(new SameAuthorRequirement()));

    options.FallbackPolicy = new AuthorizationPolicyBuilder()
                             .RequireAuthenticatedUser()
                             .Build();
});

services.AddWebOptimizer(pipeline =>
    pipeline.AddScssBundle("/css/bundle.css", "/scss/**/*.scss", "/css/**/*.css"));

services.AddSignalR();
services.AddSeeders();
services.Configure<AdminSettings>(configuration.GetSection(nameof(AdminSettings)));
var app = builder.Build();
var logger = app.Logger;

logger.LogInformation("\nConnection string: {ConnectionString} \n",
    configuration.GetConnectionString("DefaultConnection"));

logger.LogInformation("App created...");

var forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    RequireHeaderSymmetry = false
};

forwardedHeadersOptions.KnownNetworks.Clear();
forwardedHeadersOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardedHeadersOptions);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Errors/{0}");

app.UseHttpsRedirection();

app.UseWebOptimizer();

app.UseStaticFiles();

app.UseRouting();

app.UseFormHelper();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        "MyArea",
        "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        "default",
        "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapHub<SupportChatHub>("/chat");
});

await ConfigureSeeder.UseSeedersAsync(app.Services, logger, builder.Environment);
logger.LogInformation("LAUNCHING");

app.Run();