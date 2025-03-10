using OatmealDome.Slab.Mongo;

namespace OatmealDome.Slab.TestApp;

public class TestMongoDocument : SlabMongoDocument
{
    public const int LatestSchemaVersion = 2;
    
    public string Key
    {
        get;
        set;
    } = "DefaultValue";

    public string NewKey
    {
        get;
        set;
    } = "DefaultNewValue";
}
