using System.Reflection;
using EntityFrameworkCore.Triggers;
using FluentValidation.AspNetCore;
using LiWiMus.Core.Entities;
using LiWiMus.Core.Settings;
using LiWiMus.Infrastructure.Data;
using LiWiMus.Infrastructure.Data.Config;
using LiWiMus.Web.Configuration;
using LiWiMus.Web.Permission;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole();

builder.Configuration.AddEnvironmentVariables();

var services = builder.Services;

LiWiMus.Infrastructure.Dependencies.ConfigureServices(builder.Configuration, services);
builder.Services.AddTriggers();
TriggersConfiguration.ConfigureTriggers();
services.AddCoreServices(builder.Configuration);
services.AddWebServices(builder.Configuration);

services.AddIdentity(builder.Environment);
services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/Account/Login";
    options.AccessDeniedPath = "/User/Account/Denied";
});

services.AddControllersWithViews()
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

services.AddMapper();

services.AddAuthentication()
        .AddGoogle(options =>
        {
            options.ClientId = builder.Configuration.GetValue<string>("GoogleAuthSettings:ClientId");
            options.ClientSecret = builder.Configuration.GetValue<string>("GoogleAuthSettings:ClientSecret");
        });
services.AddAuthorization(options =>
{
    options.AddPolicy("SameAuthorPolicy", policyBuilder => policyBuilder.AddRequirements(new SameAuthorRequirement()));
});

services.AddWebOptimizer(pipeline =>
{
    pipeline.AddScssBundle("/css/bundle.css", "/scss/**/*.scss", "/css/**/*.css");
    //pipeline.AddJavaScriptBundle("/js/bundle.js", "/js/**/*.js");
});

var app = builder.Build();

app.Logger.LogInformation("\nConnection string: {ConnectionString} \n",
    builder.Configuration.GetConnectionString("DefaultConnection"));

app.Logger.LogInformation("App created...");

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
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Data")),
    RequestPath = "/Data"
});

app.UseRouting();

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
});

app.Logger.LogInformation("Seeding Database...");

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var applicationContext = scopedProvider.GetRequiredService<ApplicationContext>();
        var userManager = scopedProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scopedProvider.GetRequiredService<RoleManager<Role>>();
        var adminSettings = app.Configuration.GetSection("AdminSettings").Get<AdminSettings>();
        await ApplicationContextSeed.SeedAsync(applicationContext, app.Logger, userManager, roleManager, adminSettings);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Logger.LogInformation("LAUNCHING");

app.Run();