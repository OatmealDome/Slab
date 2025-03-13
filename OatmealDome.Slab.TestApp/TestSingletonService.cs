using Microsoft.Extensions.Logging;

namespace OatmealDome.Slab.TestApp;

public class TestSingletonService
{
    private readonly ILogger<TestSingletonService> _logger;
    
    public TestSingletonService(ILogger<TestSingletonService> logger)
    {
        _logger = logger;
        
        _logger.LogInformation("TestSingletonService created");
    }

    public string TestMethod()
    {
        return "TestReturn";
    }
}
