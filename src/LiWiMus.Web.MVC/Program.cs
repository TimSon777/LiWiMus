using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EntityFrameworkCore.Triggers;
using FluentValidation.AspNetCore;
using FormHelper;
using LiWiMus.Infrastructure;
using LiWiMus.Infrastructure.Data.Config;
using LiWiMus.Infrastructure.Identity;
using LiWiMus.Web.MVC.Configuration;
using LiWiMus.Web.MVC.Hubs.SupportChat;
using LiWiMus.Web.Shared.Configuration;
using LiWiMus.Web.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Logging.AddSimpleConsole();

configuration.AddEnvironmentVariables();

var services = builder.Services;

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
       .ConfigureContainer<ContainerBuilder>(containerBuilder =>
           containerBuilder.RegisterModule(new ConfigurationCoreModule(builder.Environment.ContentRootPath))
                           .RegisterModule(new ConfigurationWebModule()));

services.AddSharedServices();
services.ConfigureSettings(configuration);
Dependencies.ConfigureServices(configuration, services);
builder.Services.AddTriggers();
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
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseWebOptimizer();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Data")),
    RequestPath = "/Data"
});

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

await app.SeedDatabaseAsync(logger);

logger.LogInformation("LAUNCHING");

app.Run();