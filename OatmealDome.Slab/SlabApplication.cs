using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OatmealDome.Slab;

public abstract class SlabApplication
{
    private HostApplicationBuilder _builder;
    private IHost _host;

    protected SlabApplication()
    {
        //
    }
    
    public static void Run<T>(string[]? args) where T : SlabApplication
    {
        Type appType = typeof(T);
        T app = (T)Activator.CreateInstance(appType)!;
        
        app.Run(args);
    }

    private void Run(string[]? args)
    {
        _builder = Host.CreateApplicationBuilder(args);

        Setup();
        
        _host = _builder.Build();
        _host.Run();
    }

    protected void RegisterConfiguration<T>(string sectionName) where T : class
    {
        _builder.Services.Configure<T>(_builder.Configuration.GetSection(sectionName));
    }

    protected void RegisterHostedService<T>() where T : class, IHostedService
    {
        _builder.Services.AddHostedService<T>();
    }
    
    protected abstract void Setup();
}
