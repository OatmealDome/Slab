namespace OatmealDome.Slab.TestApp;

internal sealed class TestApplication : SlabConsoleApplication
{
    protected override void BuildApplication()
    {
        RegisterConfiguration<TestConfiguration>("Test");
        
        RegisterHostedService<TestHostedService>();
    }
}
