using System.Reflection;
using DateOnlyTimeOnly.AspNet.Converters;
using FluentValidation.AspNetCore;
using LiWiMus.Infrastructure.Data.Config;
using LiWiMus.Web.Shared.Configuration;
using LiWiMus.Web.Shared.Extensions;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using MinimalApi.Endpoint.Extensions;
using OpenIddict.Validation.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = builder.Environment.ApplicationName });
    o.CustomSchemaIds(type => type.ToString());
    o.AddFluentValidationRulesScoped();
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", builder.Environment.ApplicationName);
    c.RoutePrefix = "api/swagger";
});
app.MapEndpoints();

app.Run();

public partial class Program { }