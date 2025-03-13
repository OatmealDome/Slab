using MongoDB.Bson;
using OatmealDome.Slab.Mongo;

namespace OatmealDome.Slab.TestConsoleApp;

public class TestMongoDocumentMigratorOneToTwo : SlabMongoDocumentMigrator<TestMongoDocument>
{
    public override int OldSchemaVersion => 1;
    
    public override int NewSchemaVersion => 2;

    public override Task MigrateDocument(BsonDocument document)
    {
        document["NewKey"] = "NewValue";

        return Task.CompletedTask;
    }
}
