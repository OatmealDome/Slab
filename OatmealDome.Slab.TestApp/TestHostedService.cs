using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OatmealDome.Slab.TestApp;

internal sealed class TestHostedService : IHostedService
{
    private readonly ILogger<TestHostedService> _logger;
    
    public TestHostedService(ILogger<TestHostedService> logger)
    {
        _logger = logger;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TestHostedService -> StartAsync");
        
        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TestHostedService -> StopAsync");
        
        return Task.CompletedTask;
    }
}
