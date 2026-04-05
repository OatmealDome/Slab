using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace OatmealDome.Slab.Mongo;

public sealed class SlabMongoService : IHostedService
{
    private readonly SlabMongoRegistry _registry;
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly ILogger<SlabMongoDocumentMigrationManager> _migrationLogger;

    public SlabMongoService(IOptions<SlabMongoConfiguration> options, SlabMongoRegistry registry,
        ILogger<SlabMongoDocumentMigrationManager> migrationLogger)
    {
        SlabMongoConfiguration configuration = options.Value;

        _client = new MongoClient(configuration.ConnectionString);
        _database = _client.GetDatabase(configuration.Database);
        _registry = registry;
        _migrationLogger = migrationLogger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (KeyValuePair<Type, string> pair in _registry.CollectionNames)
        {
            IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument>(pair.Value);

            SlabMongoDocumentMigrationManager manager = new SlabMongoDocumentMigrationManager(_migrationLogger);

            if (_registry.Migrators.TryGetValue(pair.Key, out List<SlabMongoDocumentMigrator>? migrators))
            {
                foreach (SlabMongoDocumentMigrator migrator in migrators)
                {
                    manager.RegisterMigrator(migrator);
                }
            }

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
        if (!_registry.CollectionNames.ContainsKey(typeof(T)))
        {
            throw new SlabException($"No collection registered for type {nameof(T)}");
        }

        return _database.GetCollection<T>(collectionName);
    }
}
