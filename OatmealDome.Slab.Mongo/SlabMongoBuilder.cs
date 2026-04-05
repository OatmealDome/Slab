namespace OatmealDome.Slab.Mongo;

public class SlabMongoBuilder : ISlabMongoBuilder
{
    internal readonly SlabMongoRegistry Registry = new SlabMongoRegistry();
    
    internal SlabMongoBuilder()
    {
        //
    }

    public ISlabMongoBuilder AddCollection<T>(string collectionName) where T : SlabMongoDocument, new()
    {
        Type type = typeof(T);

        if (Registry.CollectionNames.ContainsKey(type))
        {
            throw new SlabException($"Collection for type {type.Name} is already registered");
        }

        Registry.CollectionNames[type] = collectionName;
        Registry.Migrators[type] = new List<SlabMongoDocumentMigrator>();

        return this;
    }

    public ISlabMongoBuilder AddMigrator<TDocument, TMigrator>()
        where TDocument : SlabMongoDocument, new()
        where TMigrator : SlabMongoDocumentMigrator<TDocument>, new()
    {
        Type docType = typeof(TDocument);

        if (!Registry.Migrators.ContainsKey(docType))
        {
            throw new SlabException(
                $"No collection registered for type {docType.Name}. Call AddCollection first.");
        }

        Registry.Migrators[docType].Add(new TMigrator());

        return this;
    }
    
}
