using System;
using System.Linq;
using DateOnlyTimeOnly.AspNet.Converters;
using LiWiMus.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LiWiMus.Web.API.Tests.Setup;

public class WebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHostBuilder? CreateHostBuilder()
    {
        Host
            .CreateDefaultBuilder()
            .ConfigureWebHostDefaults(a => a
                .UseStartup<Program>()
                .UseTestServer());
        
        return base.CreateHostBuilder();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationContext>();
            var logger = scopedServices.GetRequiredService<ILogger<WebApplicationFactory>>();
            var isDeleted = db.Database.EnsureDeleted();
            logger.LogInformation(isDeleted ? "Database was removed" : "Database wasn't removed");
        });
        
        base.ConfigureWebHost(builder);
    }
}