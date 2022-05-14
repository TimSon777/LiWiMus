using System;
using System.Linq;
using LiWiMus.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
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
        builder.ConfigureServices(services =>
        {
            var descriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationContext>));

            if (descriptor is null)
            {
                throw new SystemException();
            }
            
            services.Remove(descriptor);

            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryTestingDb");
            });

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationContext>();
            var logger = scopedServices
                .GetRequiredService<ILogger<WebApplicationFactory>>();

            db.Database.EnsureCreated();
        });
        
        base.ConfigureWebHost(builder);
    }
}