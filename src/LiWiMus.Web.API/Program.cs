using System.Reflection;
using DateOnlyTimeOnly.AspNet.Converters;
using FluentValidation.AspNetCore;
using LiWiMus.Infrastructure.Data.Config;
using LiWiMus.Web.Shared.Configuration;
using LiWiMus.Web.Shared.Extensions;
using Microsoft.AspNetCore.Http.Json;
using MinimalApi.Endpoint.Extensions;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddSharedSettings(builder.Environment);
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddCoreServices();
builder.Services.AddSharedServices();
builder.Services.ConfigureSettings(builder.Configuration);
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
TriggersConfiguration.ConfigureTriggers();
builder.Services.AddIdentity(builder.Environment);
builder.Services.AddFluentValidation(fv =>
{
    fv.LocalizationEnabled = false;
    fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});
builder.Services.AddEndpoints();
builder.Services.Configure<JsonOptions>(
    options =>
    {
        options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.SerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    });

builder.Services.AddAuthentication(options =>
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

builder.Services
       .AddOpenIddict()
       .AddValidation(options =>
       {
           options.SetIssuer("https://localhost:5021");
           options.UseSystemNetHttp();
           options.UseAspNetCore();
       });

// TODO: Remove cors
builder.Services.AddCors(options => options
    .AddDefaultPolicy(policyBuilder => policyBuilder
                                       .AllowAnyHeader()
                                       .AllowAnyMethod()
                                       .AllowAnyOrigin()));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

app.MapEndpoints();

app.Run();