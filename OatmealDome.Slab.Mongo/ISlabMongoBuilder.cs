namespace OatmealDome.Slab.Mongo;

public interface ISlabMongoBuilder
{
    ISlabMongoBuilder AddCollection<T>(string collectionName) where T : SlabMongoDocument, new();

    ISlabMongoBuilder AddMigrator<TDocument, TMigrator>()
        where TDocument : SlabMongoDocument, new()
        where TMigrator : SlabMongoDocumentMigrator<TDocument>, new();
}
