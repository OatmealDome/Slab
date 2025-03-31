using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OatmealDome.Slab.S3;

namespace OatmealDome.Slab.TestConsoleApp;

internal sealed class TestHostedService : IHostedService
{
    private readonly ILogger<TestHostedService> _logger;
    private readonly TestConfiguration _settings;
    private readonly TestSingletonService _singleton;
    private readonly TestMongoService _mongoService;
    private readonly SlabS3Service _s3Service;

    public TestHostedService(ILogger<TestHostedService> logger, IOptions<TestConfiguration> settings,
        TestSingletonService singleton, TestMongoService mongoService, SlabS3Service s3Service)
    {
        _logger = logger;
        _settings = settings.Value;
        _singleton = singleton;
        _mongoService = mongoService;
        _s3Service = s3Service;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TestHostedService -> StartAsync");
        _logger.LogInformation("TestConfiguration -> Key = {value}", _settings.Key);
        _logger.LogInformation("TestConfiguration -> OverriddenByLocalKey = {value}", _settings.OverriddenByLocalKey);
        
        _logger.LogInformation("TestSingletonService.TestMethod() -> {value}", _singleton.TestMethod());
        
        // Mongo

        /*IMongoCollection<TestMongoDocument> documents = _mongoService.GetCollection<TestMongoDocument>("test_collection");
        TestMongoDocument? document = documents.AsQueryable().FirstOrDefault();
        _logger.LogInformation("TestMongoService -> _id = {_id}",  document?._id);
        _logger.LogInformation("TestMongoService -> Key = {value}",  document?.Key);
        _logger.LogInformation("TestMongoService -> NewKey = {value}",  document?.NewKey);*/
        
        // S3
        
        /*byte[] randomBytes = new byte[4096];
        
        Random random = new Random();
        random.NextBytes(randomBytes);
        
        const string remoteName = "test.bin";

        _s3Service.TransferFile(remoteName, randomBytes, "application/octet-stream");

        byte[] downloadBytes = await _s3Service.DownloadFile(remoteName);

        _logger.LogInformation("SlabS3Service -> Uploaded and downloaded is the same: {result}",
            randomBytes.SequenceEqual(downloadBytes));*/
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TestHostedService -> StopAsync");
        
        return Task.CompletedTask;
    }
}
