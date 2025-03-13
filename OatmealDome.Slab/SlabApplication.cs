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
    protected SlabApplication()
    {
        //
    }

    internal override void Run(string[]? args)
    {
        TBuilder builder = CreateBuilder(args);
        
        SlabSerilogConfiguration slabLoggerConfiguration =
            builder.Configuration.GetSection("Serilog").Get<SlabSerilogConfiguration>()!;

        LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
            .WriteTo.Logger(Log.Logger);
            
        string slackWebhookUrl = slabLoggerConfiguration.SlackWebhookUrl;
        if (slackWebhookUrl != SlabSerilogConfiguration.DefaultSlackWebhookUrl)
        {
            loggerConfiguration = loggerConfiguration.WriteTo.Async(c =>
                c.Slack(slackWebhookUrl, restrictedToMinimumLevel: LogEventLevel.Warning));
        }
        
        Log.Logger = loggerConfiguration.CreateLogger();

        builder.Services.AddSerilog();
        
        Log.Information("Building application");

        SlabApplicationBuilder applicationBuilder = new SlabApplicationBuilder(builder);
        BuildApplication(applicationBuilder);
        
        builder.Services.AddQuartz(q =>
        {
            foreach ((Type type, JobKey jobKey, Action<ITriggerConfigurator> configurator) jobInfo in applicationBuilder.RegisteredJobs)
            {
                q.AddJob(jobInfo.type, jobInfo.jobKey);
                q.AddTrigger(t =>
                {
                    jobInfo.configurator.Invoke(t.ForJob(jobInfo.jobKey));
                });
            }
        });
        
        // Quartz.Extensions.Hosting hosting
        builder.Services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        THost host = CreateHost(builder);
        
        Log.Information("Setting up application");
        
        SetupApplication(host);
        
        Log.Warning($"{this.GetType().Name} is starting up");
        
        host.Run();
    }
    
    protected abstract TBuilder CreateBuilder(string[]? args);
    
    protected abstract THost CreateHost(TBuilder builder);
    
    protected abstract void BuildApplication(ISlabApplicationBuilder appBuilder);
    
    protected abstract void SetupApplication(THost host);
}
