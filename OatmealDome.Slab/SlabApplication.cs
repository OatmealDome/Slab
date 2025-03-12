using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Slack;

namespace OatmealDome.Slab;

public abstract class SlabApplication<TBuilder, THost> : SlabApplicationBase
    where TBuilder : IHostApplicationBuilder where THost : IHost
{
    private TBuilder? _builder;
    private THost? _host;

    private List<(Type, JobKey, Action<ITriggerConfigurator>)> _registeredJobs =
        new List<(Type, JobKey, Action<ITriggerConfigurator>)>();

    internal SlabApplication()
    {
        //
    }

    internal override void Run(string[]? args)
    {
        _builder = CreateBuilder(args);
        
        SlabSerilogConfiguration slabLoggerConfiguration =
            _builder.Configuration.GetSection("Serilog").Get<SlabSerilogConfiguration>()!;

        LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
            .WriteTo.Logger(Log.Logger);
            
        string slackWebhookUrl = slabLoggerConfiguration.SlackWebhookUrl;
        if (slackWebhookUrl != SlabSerilogConfiguration.DefaultSlackWebhookUrl)
        {
            loggerConfiguration = loggerConfiguration.WriteTo.Async(c =>
                c.Slack(slackWebhookUrl, restrictedToMinimumLevel: LogEventLevel.Warning));
        }
        
        Log.Logger = loggerConfiguration.CreateLogger();

        _builder.Services.AddSerilog();
        
        Log.Information("Building application");

        BuildApplication();
        
        _builder.Services.AddQuartz(q =>
        {
            foreach ((Type type, JobKey jobKey, Action<ITriggerConfigurator> configurator) jobInfo in _registeredJobs)
            {
                q.AddJob(jobInfo.type, jobInfo.jobKey);
                q.AddTrigger(t =>
                {
                    jobInfo.configurator.Invoke(t.ForJob(jobInfo.jobKey));
                });
            }
        });
        
        // Quartz.Extensions.Hosting hosting
        _builder.Services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        _host = CreateHost(_builder);
        
        Log.Information("Setting up application");
        
        SetupApplication(_host);
        
        Log.Warning($"{this.GetType().Name} is starting up");
        
        _host.Run();
    }

    protected void RegisterConfiguration<T>(string sectionName) where T : class
    {
        _builder!.Services.Configure<T>(_builder.Configuration.GetSection(sectionName));
    }

    protected void RegisterHostedService<T>() where T : class, IHostedService
    {
        _builder!.Services
            .AddSingleton<T>()
            .AddHostedService(services => services.GetService<T>()!);
    }

    protected void RegisterJob<T>(JobKey jobKey, Action<ITriggerConfigurator> configurator) where T : SlabJob
    {
        Type jobType = typeof(T);

        if (jobType == typeof(SlabJob))
        {
            throw new SlabException("Specified class must be subclass of SlabJob");
        }
        
        _registeredJobs.Add((typeof(T), jobKey, configurator));
    }
    
    protected abstract TBuilder CreateBuilder(string[]? args);
    
    protected abstract THost CreateHost(TBuilder builder);
    
    protected abstract void BuildApplication();
    
    protected abstract void SetupApplication(THost host);
}
