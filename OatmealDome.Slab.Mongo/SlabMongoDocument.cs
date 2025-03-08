using MongoDB.Bson;

namespace OatmealDome.Slab.Mongo;

public abstract class SlabMongoDocument
{
    // ReSharper disable once InconsistentNaming
    public ObjectId _id
    {
        get;
        set;
    } = ObjectId.Empty;

    public int SchemaVersion
    {
        get;
        set;
    } = 0;
}
