using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OatmealDome.Slab.TestApp;

internal sealed class TestHostedService : IHostedService
{
    private readonly ILogger<TestHostedService> _logger;
    private readonly TestConfiguration _settings;
    
    public TestHostedService(ILogger<TestHostedService> logger, IOptions<TestConfiguration> settings)
    {
        _logger = logger;
        _settings = settings.Value;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TestHostedService -> StartAsync");
        _logger.LogInformation("TestConfiguration -> Key = {value}", _settings.Key);
        
        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TestHostedService -> StopAsync");
        
        return Task.CompletedTask;
    }
}
