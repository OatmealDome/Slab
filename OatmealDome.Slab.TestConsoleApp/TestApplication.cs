using OatmealDome.Slab.Mongo;

namespace OatmealDome.Slab.TestConsoleApp;

internal sealed class TestApplication : SlabConsoleApplication
{
    protected override string ApplicationName => "TestConsoleApp";

    protected override void BuildApplication(ISlabApplicationBuilder builder)
    {
        builder.RegisterMongo<TestMongoService>();
        
        builder.RegisterSingleton<TestSingletonService>();
        
        builder.RegisterConfiguration<TestConfiguration>("Test");
        builder.RegisterHostedService<TestHostedService>();

        builder.RegisterJob<TestJob>(SlabJob.CreateJobKey("TestJob"), t => t
            .StartAt(DateTime.UtcNow.AddSeconds(10)));
    }
}
