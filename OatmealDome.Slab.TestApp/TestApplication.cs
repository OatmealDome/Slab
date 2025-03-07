using OatmealDome.Slab.Mongo;

namespace OatmealDome.Slab.TestApp;

internal sealed class TestApplication : SlabConsoleApplication
{
    protected override void BuildApplication()
    {
        RegisterConfiguration<TestConfiguration>("Test");
        RegisterConfiguration<SlabMongoConfiguration>("Mongo");
        
        RegisterHostedService<TestMongoService>();
        RegisterHostedService<TestHostedService>();
    }
}
