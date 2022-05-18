using System.Reflection;
using FluentValidation.AspNetCore;
using HashidsNet;
using LiWiMus.Web.FileServer;
using LiWiMus.Web.Shared.Configuration;
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

builder.Services.AddSwaggerWithAuthorize(builder.Environment.ApplicationName);

var app = builder.Build();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", builder.Environment.ApplicationName);
    c.RoutePrefix = "api/swagger";
});

app.MapControllers();

var filesPath = Path.Combine(builder.Environment.ContentRootPath, "Files");
Directory.CreateDirectory(filesPath);

app.Run();