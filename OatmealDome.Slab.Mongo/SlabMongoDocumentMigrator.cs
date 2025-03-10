using MongoDB.Bson;

namespace OatmealDome.Slab.Mongo;

public abstract class SlabMongoDocumentMigrator
{
    public abstract int OldSchemaVersion
    {
        get;
    }

    public abstract int NewSchemaVersion
    {
        get;
    }

    internal SlabMongoDocumentMigrator()
    {
        //
    }
    
    public abstract Task MigrateDocument(BsonDocument document);
}


public abstract class SlabMongoDocumentMigrator<T> : SlabMongoDocumentMigrator where T : SlabMongoDocument, new()
{
    protected SlabMongoDocumentMigrator() : base()
    {
        
    }
}
