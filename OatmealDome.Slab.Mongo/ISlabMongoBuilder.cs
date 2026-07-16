namespace OatmealDome.Slab.Mongo;

public interface ISlabMongoBuilder
{
    ISlabMongoBuilder AddCollection<T>(string collectionName) where T : SlabMongoDocument;

    ISlabMongoBuilder AddMigrator<TDocument, TMigrator>()
        where TDocument : SlabMongoDocument
        where TMigrator : SlabMongoDocumentMigrator<TDocument>, new();
}
