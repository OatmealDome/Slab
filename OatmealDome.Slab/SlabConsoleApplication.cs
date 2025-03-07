using Microsoft.Extensions.Hosting;

namespace OatmealDome.Slab;

public abstract class SlabConsoleApplication : SlabApplication<HostApplicationBuilder, IHost>
{
    protected override HostApplicationBuilder CreateBuilder(string[]? args) => Host.CreateApplicationBuilder(args);
    
    protected override IHost CreateHost(HostApplicationBuilder builder) => builder.Build();

    protected override void SetupApplication(IHost host)
    {
        // Nothing needs to be done here.
    }
}
