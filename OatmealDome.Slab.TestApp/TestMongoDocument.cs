using MongoDB.Bson;
using OatmealDome.Slab.Mongo;

namespace OatmealDome.Slab.TestApp;

public class TestMongoDocument : SlabMongoDocument
{
    public const int LatestSchemaVersion = 1;
    
    public string Key
    {
        get;
        set;
    } = "DefaultValue";
}
