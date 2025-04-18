using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace OatmealDome.Slab;

public class SlabApplicationBuilder : ISlabApplicationBuilder
{
    private readonly IHostApplicationBuilder _hostBuilder;

    internal readonly List<(Type, JobKey, Action<ITriggerConfigurator>)> RegisteredJobs =
        new List<(Type, JobKey, Action<ITriggerConfigurator>)>();

    public IServiceCollection Services => _hostBuilder.Services;
    
    internal SlabApplicationBuilder(IHostApplicationBuilder hostBuilder)
    {
        _hostBuilder = hostBuilder;
    }

    public void RegisterConfiguration<T>(string sectionName) where T : class, new()
    {
        _hostBuilder.Services.Configure<T>(_hostBuilder.Configuration.GetSection(sectionName));
    }

    public void RegisterSingleton<T>() where T : class
    {
        _hostBuilder.Services.AddSingleton<T>();
    }

    public void RegisterHostedService<T>() where T : class, IHostedService
    {
        _hostBuilder.Services
            .AddSingleton<T>()
            .AddHostedService(services => services.GetService<T>()!);
    }

    public void RegisterJob<T>(SlabJobKey jobKey, Action<ITriggerConfigurator> configurator) where T : SlabJob
    {
        Type jobType = typeof(T);

        if (jobType == typeof(SlabJob))
        {
            throw new SlabException("Specified class must be subclass of SlabJob");
        }
        
        RegisteredJobs.Add((typeof(T), jobKey.QuartzJobKey, configurator));
    }
}
