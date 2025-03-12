using OatmealDome.Slab.Mongo;
using Quartz;

namespace OatmealDome.Slab.TestApp;

internal sealed class TestApplication : SlabConsoleApplication
{
    protected override void BuildApplication()
    {
        RegisterConfiguration<TestConfiguration>("Test");
        RegisterConfiguration<SlabMongoConfiguration>("Mongo");
        
        RegisterHostedService<TestMongoService>();
        RegisterHostedService<TestHostedService>();

        RegisterJob<TestJob>(SlabJob.CreateJobKey("TestJob"), t => t
            .StartAt(DateTime.UtcNow.AddSeconds(10)));
    }
}
