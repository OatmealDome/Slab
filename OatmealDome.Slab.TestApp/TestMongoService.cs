using Microsoft.Extensions.Options;
using OatmealDome.Slab.Mongo;

namespace OatmealDome.Slab.TestApp;

public class TestMongoService : SlabMongoService
{
    public TestMongoService(IOptions<SlabMongoConfiguration> options, IServiceProvider serviceProvider) : base(options,
        serviceProvider)
    {
        //
    }

    protected override void Setup()
    {
        RegisterCollection<TestMongoDocument>("test_collection");
        RegisterMigrator<TestMongoDocument, TestMongoDocumentMigratorOneToTwo>();
    }
}
