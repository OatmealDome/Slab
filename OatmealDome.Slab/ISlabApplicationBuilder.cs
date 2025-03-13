using Microsoft.Extensions.Hosting;
using Quartz;

namespace OatmealDome.Slab;

public interface ISlabApplicationBuilder
{
    void RegisterConfiguration<T>(string sectionName) where T : class, new();

    void RegisterHostedService<T>() where T : class, IHostedService;

    void RegisterJob<T>(JobKey jobKey, Action<ITriggerConfigurator> configurator) where T : SlabJob;
}
