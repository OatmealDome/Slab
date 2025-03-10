using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace OatmealDome.Slab.Mongo;

public abstract class SlabMongoService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;

    private readonly Dictionary<Type, string> _collectionNames = new Dictionary<Type, string>();
    private readonly Dictionary<Type, SlabMongoDocumentMigrationManager> _migrationManagers =
        new Dictionary<Type, SlabMongoDocumentMigrationManager>();
    
    protected SlabMongoService(IOptions<SlabMongoConfiguration> options, IServiceProvider serviceProvider)
    {
        SlabMongoConfiguration configuration = options.Value;
        
        _client = new MongoClient(configuration.ConnectionString);
        _database = _client.GetDatabase(configuration.Database);
        
        _serviceProvider = serviceProvider;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Setup();

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
    
    protected void RegisterCollection<T>(string collectionName) where T : SlabMongoDocument, new()
    {
        Type type = typeof(T);
        
        if (_collectionNames.ContainsKey(type))
        {
            throw new SlabException($"A collection for {nameof(T)} is already registered.");
        }
        
        _collectionNames[type] = collectionName;
        _migrationManagers[type] =
            new SlabMongoDocumentMigrationManager(_serviceProvider
                .GetService<ILogger<SlabMongoDocumentMigrationManager>>()!);
    }

    protected void RegisterMigrator<TDocument, TMigrator>() where TDocument : SlabMongoDocument, new()
        where TMigrator : SlabMongoDocumentMigrator<TDocument>, new()
    {
        if (!_migrationManagers.TryGetValue(typeof(TDocument), out var manager))
        {
            throw new SlabException($"No collection registered for type {nameof(TDocument)}");
        }
        
        manager.RegisterMigrator<TDocument, TMigrator>();
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName) where T : SlabMongoDocument
    {
        if (!_collectionNames.ContainsKey(typeof(T)))
        {
            throw new SlabException($"No collection registered for type {nameof(T)}");
        }
        
        return _database.GetCollection<T>(collectionName);
    }
    
    protected abstract void Setup();
}
