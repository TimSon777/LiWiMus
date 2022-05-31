using System.Reflection;
using DateOnlyTimeOnly.AspNet.Converters;
using FluentValidation.AspNetCore;
using LiWiMus.Core.Roles;
using LiWiMus.Core.Settings;
using LiWiMus.Infrastructure.Data.Config;
using LiWiMus.Web.Shared.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.IdentityModel.Tokens;
using MinimalApi.Endpoint.Extensions;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpoints();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
        .RequireClaim(SystemPermission.ClaimType, DefaultSystemPermissions.Admin.Access.Name)
        .Build();
});

builder.Services
       .AddOpenIddict()
       .AddValidation(options =>
       {
           options.SetIssuer("http://localhost:5020");
           options.UseSystemNetHttp();
           options.UseAspNetCore();
           options.Configure(a => a.TokenValidationParameters.IssuerSigningKey =
               new SymmetricSecurityKey(
                   Convert.FromBase64String(builder.Configuration["SigninKey"])));
       });

builder.Services.AddSeeders();

builder.AddMicroServices();

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
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", builder.Environment.ApplicationName);
    c.RoutePrefix = "api/swagger";
});
app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

app.MapEndpoints();

{
    await ConfigureSeeder.UseSeedersAsync(app.Services, app.Logger, builder.Environment);
}

app.Run();

//For tests
// ReSharper disable once UnusedType.Global
public partial class Program
{
}