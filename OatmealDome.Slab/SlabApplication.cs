using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OatmealDome.Slab;

public abstract class SlabApplication<TBuilder, THost> : SlabApplicationBase
    where TBuilder : IHostApplicationBuilder where THost : IHost
{    
    private TBuilder? _builder;
    private THost? _host;

    internal SlabApplication()
    {
        //
    }

    internal override void Run(string[]? args)
    {
        _builder = CreateBuilder(args);

        BuildApplication();

        _host = CreateHost(_builder);
        
        SetupApplication(_host);
        
        _host.Run();
    }

    protected void RegisterConfiguration<T>(string sectionName) where T : class
    {
        _builder!.Services.Configure<T>(_builder.Configuration.GetSection(sectionName));
    }

    protected void RegisterHostedService<T>() where T : class, IHostedService
    {
        _builder!.Services.AddHostedService<T>();
    }
    
    protected abstract TBuilder CreateBuilder(string[]? args);
    
    protected abstract THost CreateHost(TBuilder builder);
    
    protected abstract void BuildApplication();
    
    protected abstract void SetupApplication(THost host);
}
