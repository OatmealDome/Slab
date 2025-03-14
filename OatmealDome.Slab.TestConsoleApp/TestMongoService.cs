using Microsoft.Extensions.Options;
using OatmealDome.Slab.Mongo;

namespace OatmealDome.Slab.TestConsoleApp;

public class TestMongoService : SlabMongoService
{
    public TestMongoService(IOptions<SlabMongoConfiguration> options, IServiceProvider serviceProvider) : base(options,
        serviceProvider)
    {
        //
    }

    protected override void Setup(ISlabMongoCollectionsBuilder collectionsBuilder)
    {
        collectionsBuilder.RegisterCollection<TestMongoDocument>("test_collection");
        collectionsBuilder.RegisterMigrator<TestMongoDocument, TestMongoDocumentMigratorOneToTwo>();
    }
}
