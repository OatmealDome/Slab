using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace OatmealDome.Slab;

public interface ISlabApplicationBuilder
{
    IServiceCollection Services
    {
        get;
    }
    
    void RegisterConfiguration<T>(string sectionName) where T : class, new();

    void RegisterSingleton<T>() where T : class;

    void RegisterHostedService<T>() where T : class, IHostedService;

    void RegisterJob<T>(JobKey jobKey, Action<ITriggerConfigurator> configurator) where T : SlabJob;
}
