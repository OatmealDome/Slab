namespace OatmealDome.Slab.Mongo;

public class SlabMongoBuilder : ISlabMongoBuilder
{
    internal readonly SlabMongoRegistry Registry = new SlabMongoRegistry();
    
    internal SlabMongoBuilder()
    {
        //
    }

    public ISlabMongoBuilder AddCollection<T>(string collectionName) where T : SlabMongoDocument
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
        where TDocument : SlabMongoDocument
        where TMigrator : SlabMongoDocumentMigrator<TDocument>, new()
    {
        Type documentType = typeof(TDocument);
        Type[] matchingCollectionTypes = Registry.CollectionNames.Keys
            .Where(collectionType => collectionType.IsAssignableFrom(documentType))
            .ToArray();

        if (matchingCollectionTypes.Length == 0)
        {
            throw new SlabException(
                $"No collection registered for type {documentType.Name} or one of its base types. Call AddCollection first.");
        }

        if (matchingCollectionTypes.Length > 1)
        {
            throw new SlabException($"More than one registered collection can contain {documentType.Name}: " +
                                    string.Join(", ", matchingCollectionTypes.Select(t => t.Name)));
        }

        Type collectionType = matchingCollectionTypes[0];
        Registry.Migrators[collectionType].Add(new TMigrator());

        return this;
    }
    
}
