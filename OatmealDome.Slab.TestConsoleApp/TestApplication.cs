using OatmealDome.Slab.Mongo;
using OatmealDome.Slab.S3;
using Quartz;

namespace OatmealDome.Slab.TestConsoleApp;

internal sealed class TestApplication : SlabConsoleApplication
{
    protected override string ApplicationName => "TestConsoleApp";

    protected override void BuildApplication(ISlabApplicationBuilder builder)
    {
        builder.RegisterMongo<TestMongoService>();
        
        builder.RegisterS3();
        
        builder.RegisterSingleton<TestSingletonService>();
        
        builder.RegisterConfiguration<TestConfiguration>("Test");
        builder.RegisterHostedService<TestHostedService>();

        builder.RegisterJob<TestJob>(SlabJobKey.Create("TestJob"), t => t
            .StartAt(DateTime.UtcNow.AddSeconds(10)));
    }
}
