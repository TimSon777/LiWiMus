using System.Reflection;
using FluentValidation.AspNetCore;
using LiWiMus.Infrastructure.Data;
using LiWiMus.Web.Configuration;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole();

builder.Configuration.AddEnvironmentVariables();

var services = builder.Services;

LiWiMus.Infrastructure.Dependencies.ConfigureServices(builder.Configuration, services);
services.AddCoreServices(builder.Configuration);
services.AddWebServices(builder.Configuration);

services.AddIdentity(builder.Environment);

services.AddControllersWithViews()
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

services.AddMapper();

services.AddAuthentication()
        .AddGoogle(options =>
        {
            options.ClientId = builder.Configuration.GetValue<string>("GoogleAuthSettings:ClientId");
            options.ClientSecret = builder.Configuration.GetValue<string>("GoogleAuthSettings:ClientSecret");
        });

services.AddWebOptimizer(pipeline => { pipeline.CompileScssFiles(); });

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Logger.LogInformation("Seeding Database...");

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var applicationContext = scopedProvider.GetRequiredService<ApplicationContext>();
        await ApplicationContextSeed.SeedAsync(applicationContext, app.Logger);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Logger.LogInformation("LAUNCHING");

app.Run();