using OatmealDome.Slab.Mongo;
using Quartz;

namespace OatmealDome.Slab.TestApp;

internal sealed class TestApplication : SlabConsoleApplication
{
    protected override void BuildApplication(ISlabApplicationBuilder builder)
    {
        builder.RegisterConfiguration<TestConfiguration>("Test");
        builder.RegisterConfiguration<SlabMongoConfiguration>("Mongo");
        
        builder.RegisterHostedService<TestMongoService>();
        builder.RegisterHostedService<TestHostedService>();

        builder.RegisterJob<TestJob>(SlabJob.CreateJobKey("TestJob"), t => t
            .StartAt(DateTime.UtcNow.AddSeconds(10)));
    }
}
