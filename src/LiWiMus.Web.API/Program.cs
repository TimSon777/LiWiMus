using System.Reflection;
using DateOnlyTimeOnly.AspNet.Converters;
using FluentValidation.AspNetCore;
using LiWiMus.Core.Settings;
using LiWiMus.Infrastructure.Data.Config;
using LiWiMus.Web.API;
using LiWiMus.Web.Shared.Configuration;
using LiWiMus.Web.Shared.Extensions;
using Microsoft.AspNetCore.Http.Json;
using MinimalApi.Endpoint.Extensions;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpoints();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (builder.Environment.EnvironmentName == "Testing")
{
    connectionString = connectionString.Replace("{Database}", Guid.NewGuid().ToString());
}

builder.Services.AddDbContext(connectionString);
builder.Services.AddCoreServices();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
TriggersConfiguration.ConfigureTriggers();
builder.Services.AddIdentity(builder.Environment);
builder.Services.AddFluentValidation(fv =>
{
    fv.LocalizationEnabled = false;
    fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.Configure<JsonOptions>(
    options =>
    {
        options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.SerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    });

builder.Services.Configure<AdminSettings>(builder.Configuration.GetSection(nameof(AdminSettings)));

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

builder.Services.AddSeeders();

// TODO: Remove cors
builder.Services.AddCors(options => options
    .AddDefaultPolicy(policyBuilder => policyBuilder
                                       .AllowAnyHeader()
                                       .AllowAnyMethod()
                                       .AllowAnyOrigin()));

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(o =>
// {
//     o.SwaggerDoc("v1", new OpenApiInfo {Title = builder.Environment.ApplicationName});
//     o.CustomSchemaIds(type => type.ToString());
//     o.AddFluentValidationRulesScoped();
// });
builder.Services.AddSwaggerWithAuthorize(builder.Environment.ApplicationName);
builder.Services.AddHostedService<HostedService>();
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

//For tests
#pragma warning disable CA1050
// ReSharper disable once UnusedType.Global
public partial class Program
#pragma warning restore CA1050
{
}