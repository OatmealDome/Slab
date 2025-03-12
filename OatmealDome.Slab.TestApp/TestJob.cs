using Microsoft.Extensions.Logging;
using Quartz;

namespace OatmealDome.Slab.TestApp;

public class TestJob : SlabJob
{
    private readonly ILogger<TestJob> _logger;
    
    public TestJob(ILogger<TestJob> logger)
    {
        _logger = logger;
    }
    
    protected override Task Run(IJobExecutionContext context)
    {
        throw new Exception("Test exception");
        _logger.LogInformation("TestJob executed");
        
        return Task.CompletedTask;
    }
}
