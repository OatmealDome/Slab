using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace OatmealDome.Slab.TestApp;

internal sealed class TestHostedService : IHostedService
{
    private readonly ILogger<TestHostedService> _logger;
    private readonly TestConfiguration _settings;
    private readonly TestMongoService _mongoService;
    
    public TestHostedService(ILogger<TestHostedService> logger, IOptions<TestConfiguration> settings, TestMongoService mongoService)
    {
        _logger = logger;
        _settings = settings.Value;
        _mongoService = mongoService;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TestHostedService -> StartAsync");
        _logger.LogInformation("TestConfiguration -> Key = {value}", _settings.Key);

        IMongoCollection<TestMongoDocument> documents = _mongoService.GetCollection<TestMongoDocument>("test_collection");
        TestMongoDocument? document = documents.AsQueryable().FirstOrDefault();
        _logger.LogInformation("TestMongoService -> _id = {_id}",  document?._id);
        _logger.LogInformation("TestMongoService -> Key = {value}",  document?.Key);
        _logger.LogInformation("TestMongoService -> NewKey = {value}",  document?.NewKey);
        
        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TestHostedService -> StopAsync");
        
        return Task.CompletedTask;
    }
}
