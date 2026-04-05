namespace OatmealDome.Slab.Mongo;

public class SlabMongoRegistry
{
    internal Dictionary<Type, string> CollectionNames
    {
        get;
    } = new Dictionary<Type, string>();

    internal Dictionary<Type, List<SlabMongoDocumentMigrator>> Migrators
    {
        get;
    } = new Dictionary<Type, List<SlabMongoDocumentMigrator>>();
}
