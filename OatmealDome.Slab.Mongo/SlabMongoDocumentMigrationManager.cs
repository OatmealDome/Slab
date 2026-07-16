using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace OatmealDome.Slab.Mongo;

public class SlabMongoDocumentMigrationManager
{
    private readonly ILogger _logger;
    private readonly List<SlabMongoDocumentMigrator> _migrators = new List<SlabMongoDocumentMigrator>();

    public SlabMongoDocumentMigrationManager(ILogger<SlabMongoDocumentMigrationManager> logger)
    {
        _logger = logger;
    }
    
    public async Task PerformMigration(IMongoCollection<BsonDocument> collection)
    {
        foreach (BsonDocument document in collection.AsQueryable())
        {
            foreach (SlabMongoDocumentMigrator migrator in _migrators)
            {
                Type documentType = migrator.DocumentType;

                if (!documentType.IsAbstract && documentType.BaseType != null)
                {
                    BsonArray typeArray = document["_t"].AsBsonArray;

                    // Ensure this document is of the right type before migrating it.
                    if (documentType.Name != typeArray.Last().AsString)
                    {
                        continue;
                    }
                }

                int schemaVersion = document["SchemaVersion"].AsInt32;

                if (migrator.OldSchemaVersion != schemaVersion)
                {
                    continue;
                }

                BsonElement documentId = document.GetElement("_id");

                try
                {
                    await migrator.MigrateDocument(document);

                    document["SchemaVersion"] = migrator.NewSchemaVersion;

                    await collection.ReplaceOneAsync(
                        Builders<BsonDocument>.Filter.Eq("_id", documentId.Value.AsObjectId), document);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Migration failed on document {DocumentId}", documentId);
                    continue;
                }

                _logger.LogInformation("Migrated document {DocumentId} with {Migrator}", documentId,
                    migrator.GetType().Name);
            }
        }
    }

    internal void RegisterMigrator(SlabMongoDocumentMigrator migrator)
    {
        _migrators.Add(migrator);
    }

    public void RegisterMigrator<TDocument, TMigrator>()  where TDocument : SlabMongoDocument
        where TMigrator : SlabMongoDocumentMigrator<TDocument>, new()
    {
        Type migratorType = typeof(TMigrator);
        TMigrator migrator = (TMigrator)Activator.CreateInstance(migratorType)!;
        
        _migrators.Add(migrator);
    }
}
