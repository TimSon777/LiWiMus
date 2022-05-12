using System.Reflection;
using FluentValidation.AspNetCore;
using HashidsNet;
using LiWiMus.Web.FileServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FileContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var hashIdsSalt = builder.Configuration.GetRequiredSection("HashIdsSecret").Get<string>();
builder.Services.AddSingleton<IHashids>(_ => new Hashids(hashIdsSalt));

builder.Services.AddControllers();
builder.Services.AddFluentValidation(options =>
{
    options.LocalizationEnabled = false;
    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var filesPath = Path.Combine(builder.Environment.ContentRootPath, "Files");
Directory.CreateDirectory(filesPath);

app.Run();