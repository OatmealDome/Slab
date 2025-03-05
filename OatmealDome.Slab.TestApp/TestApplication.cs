namespace OatmealDome.Slab.TestApp;

internal sealed class TestApplication : SlabApplication
{
    protected override void Setup()
    {
        RegisterConfiguration<TestConfiguration>("Test");
        
        RegisterHostedService<TestHostedService>();
    }
}
