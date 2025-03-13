using OatmealDome.Slab.Mongo;

namespace OatmealDome.Slab.TestConsoleApp;

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
