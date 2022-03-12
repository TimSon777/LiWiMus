using LiWiMus.Core.Entities;
using LiWiMus.Infrastructure.Data;
using LiWiMus.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole(options => options.FormatterName = "simple");

builder.Configuration.AddEnvironmentVariables();

var services = builder.Services;

LiWiMus.Infrastructure.Dependencies.ConfigureServices(builder.Configuration, services);
services.AddCoreServices(builder.Configuration);

services.AddDefaultIdentity<User>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 3;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<ApplicationContext>();
services.AddControllersWithViews();

var app = builder.Build();

app.Logger.LogInformation("\nConnection string: {ConnectionString} \n", builder.Configuration.GetConnectionString("DefaultConnection"));

app.Logger.LogInformation("App created...");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

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