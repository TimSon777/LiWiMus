using LiWiMus.Infrastructure;
using LiWiMus.Infrastructure.Data;
using LiWiMus.Infrastructure.Data.Seeders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LiWiMus.Web.Shared.Configuration;

public class HostedService : IHostedService
{
    private readonly IWebHostEnvironment _environment;
    private readonly ApplicationContext _applicationContext;
    private readonly IEnumerable<ISeeder> _seeders;
    private readonly ILogger _logger;

    public HostedService(IWebHostEnvironment environment, 
        ApplicationContext applicationContext, 
        IEnumerable<ISeeder> seeders,
        ILogger<HostedService> logger)
    {
        _environment = environment;
        _applicationContext = applicationContext;
        _seeders = seeders;
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Seeding Database...");
        
        try
        {
            var envType = Enum.Parse<EnvironmentType>(_environment.EnvironmentName);
            await ApplicationContextSeed.SeedAsync(_applicationContext, envType, _logger, _seeders.ToArray());
            await ApplicationContextClear.ClearAsync(_applicationContext, _logger, _environment.IsDevelopment());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred seeding the DB");
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}