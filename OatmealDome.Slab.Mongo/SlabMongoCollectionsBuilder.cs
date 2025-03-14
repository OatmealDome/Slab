using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace OatmealDome.Slab.Mongo;

public class SlabMongoCollectionsBuilder : ISlabMongoCollectionsBuilder
{
    private readonly IServiceProvider _serviceProvider;
    
    internal readonly Dictionary<Type, string> CollectionNames = new Dictionary<Type, string>();
    internal readonly Dictionary<Type, SlabMongoDocumentMigrationManager> MigrationManagers =
        new Dictionary<Type, SlabMongoDocumentMigrationManager>();

    public SlabMongoCollectionsBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public void RegisterCollection<T>(string collectionName) where T : SlabMongoDocument, new()
    {
        Type type = typeof(T);
        
        if (CollectionNames.ContainsKey(type))
        {
            throw new SlabException($"A collection for {nameof(T)} is already registered.");
        }
        
        CollectionNames[type] = collectionName;
        MigrationManagers[type] =
            new SlabMongoDocumentMigrationManager(_serviceProvider
                .GetService<ILogger<SlabMongoDocumentMigrationManager>>()!);
    }

    public void RegisterMigrator<TDocument, TMigrator>() where TDocument : SlabMongoDocument, new()
        where TMigrator : SlabMongoDocumentMigrator<TDocument>, new()
    {
        if (!MigrationManagers.TryGetValue(typeof(TDocument), out var manager))
        {
            throw new SlabException($"No collection registered for type {nameof(TDocument)}");
        }
        
        manager.RegisterMigrator<TDocument, TMigrator>();
    }
}
