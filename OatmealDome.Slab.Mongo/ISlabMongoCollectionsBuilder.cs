namespace OatmealDome.Slab.Mongo;

public interface ISlabMongoCollectionsBuilder
{
    void RegisterCollection<T>(string collectionName) where T : SlabMongoDocument, new();

    void RegisterMigrator<TDocument, TMigrator>() where TDocument : SlabMongoDocument, new()
        where TMigrator : SlabMongoDocumentMigrator<TDocument>, new();
}
