using Microsoft.Extensions.Hosting;

namespace OatmealDome.Slab;

public abstract class SlabApplication
{
    protected SlabApplication()
    {
        //
    }
    
    public static void Run<T>(string[]? args) where T : SlabApplication
    {
        Type appType = typeof(T);
        T app = (T)Activator.CreateInstance(appType)!;
        
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        
        IHost host = builder.Build();
        host.Run();
    }
}
