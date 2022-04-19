using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EntityFrameworkCore.Triggers;
using FluentValidation.AspNetCore;
using FormHelper;
using LiWiMus.Core.Settings;
using LiWiMus.Core.Users;
using LiWiMus.Infrastructure;
using LiWiMus.Infrastructure.Data;
using LiWiMus.Infrastructure.Data.Config;
using LiWiMus.Infrastructure.Identity;
using LiWiMus.Web.Configuration;
using LiWiMus.Web.Hubs.SupportChat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
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

services.Configure<DataSettings>(configuration.GetSection("DataSettings"));
services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

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

services.AddMapper();

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
        name: "MyArea",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapHub<SupportChatUserHub>("/chat");
});

logger.LogInformation("Seeding Database...");

using var scope = app.Services.CreateScope();
var scopedProvider = scope.ServiceProvider;

try
{
    var applicationContext = scopedProvider.GetRequiredService<ApplicationContext>();
    var userManager = scopedProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var adminSettings = app.Configuration.GetSection("AdminSettings").Get<AdminSettings>();
    await ApplicationContextSeed.SeedAsync(applicationContext, logger, userManager, roleManager, adminSettings);
    await ApplicationContextClear.ClearAsync(applicationContext, logger);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred seeding the DB.");
}

logger.LogInformation("LAUNCHING");

app.Run();