using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace OatmealDome.Slab.Mongo;

public abstract class SlabMongoService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;

    private Dictionary<Type, string> _collectionNames;
    private Dictionary<Type, SlabMongoDocumentMigrationManager> _migrationManagers;
    
    protected SlabMongoService(IOptions<SlabMongoConfiguration> options, IServiceProvider serviceProvider)
    {
        SlabMongoConfiguration configuration = options.Value;
        
        _client = new MongoClient(configuration.ConnectionString);
        _database = _client.GetDatabase(configuration.Database);
        
        _serviceProvider = serviceProvider;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        SlabMongoCollectionsBuilder collectionsBuilder = new SlabMongoCollectionsBuilder(_serviceProvider);
        
        BuildService(collectionsBuilder);

        _collectionNames = collectionsBuilder.CollectionNames;
        _migrationManagers = collectionsBuilder.MigrationManagers;

        foreach (KeyValuePair<Type, string> pair in _collectionNames)
        {
            IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument>(pair.Value);
            
            SlabMongoDocumentMigrationManager manager = _migrationManagers[pair.Key];
            await manager.PerformMigration(collection);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _client.Dispose();
        
        return Task.CompletedTask;
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName) where T : SlabMongoDocument
    {
        if (!_collectionNames.ContainsKey(typeof(T)))
        {
            throw new SlabException($"No collection registered for type {nameof(T)}");
        }
        
        return _database.GetCollection<T>(collectionName);
    }
    
    protected abstract void BuildService(ISlabMongoCollectionsBuilder collectionsBuilder);
}
