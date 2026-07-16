using MongoDB.Bson;

namespace OatmealDome.Slab.Mongo;

public abstract class SlabMongoDocumentMigrator
{
    public abstract Type DocumentType
    {
        get;
    }

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


public abstract class SlabMongoDocumentMigrator<T> : SlabMongoDocumentMigrator where T : SlabMongoDocument
{
    public override Type DocumentType => typeof(T);
    
    protected SlabMongoDocumentMigrator() : base()
    {
        
    }
}
