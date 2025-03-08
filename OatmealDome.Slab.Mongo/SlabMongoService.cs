using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace OatmealDome.Slab.Mongo;

public abstract class SlabMongoService : IHostedService
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;

    protected SlabMongoService(IOptions<SlabMongoConfiguration> options)
    {
        SlabMongoConfiguration configuration = options.Value;
        
        _client = new MongoClient(configuration.ConnectionString);
        _database = _client.GetDatabase(configuration.Database);
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _client.Dispose();
        
        return Task.CompletedTask;
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName) where T : SlabMongoDocument
    {
        return _database.GetCollection<T>(collectionName);
    }
}
