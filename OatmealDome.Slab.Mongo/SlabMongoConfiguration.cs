namespace OatmealDome.Slab.Mongo;

public sealed class SlabMongoConfiguration
{
    public string ConnectionString
    {
        get;
        set;
    } = "mongodb://root:password@127.0.0.1";

    public string Database
    {
        get;
        set;
    } = "database";
}
