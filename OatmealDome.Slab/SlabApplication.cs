using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Sinks.Slack;

namespace OatmealDome.Slab;

public abstract class SlabApplication<TBuilder, THost> : SlabApplicationBase
    where TBuilder : IHostApplicationBuilder where THost : IHost
{
    protected abstract string ApplicationName
    {
        get;
    }
    
    protected SlabApplication()
    {
        //
    }

    internal override void Run(string[]? args)
    {
        using (LogContext.PushProperty("SourceContext", GetType().FullName))
        {
            Log.Warning($"{ApplicationName} is starting up");
        }

        TBuilder builder = CreateBuilder(args);

        if (builder.Environment.EnvironmentName == "Local")
        {
            throw new SlabException("Local is a reserved keyword and cannot be an environment name");
        }

        builder.Configuration.AddJsonFile($"appsettings.Local.json", true, true);
        
        SlabSerilogConfiguration? slabLoggerConfiguration =
            builder.Configuration.GetSection("Serilog").Get<SlabSerilogConfiguration>();

        LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
            .WriteTo.Logger(Log.Logger);
            
        string? slackWebhookUrl = slabLoggerConfiguration?.SlackWebhookUrl;
        if (slabLoggerConfiguration != null && slackWebhookUrl != SlabSerilogConfiguration.DefaultSlackWebhookUrl)
        {
            loggerConfiguration = loggerConfiguration.WriteTo.Async(c => c.Slack(slackWebhookUrl,
                restrictedToMinimumLevel: LogEventLevel.Warning, outputTemplate: "{Message:lj}"));
        }
        
        Log.Logger = loggerConfiguration.CreateLogger();

        builder.Services.AddSerilog();
        
        using (LogContext.PushProperty("SourceContext", GetType().FullName))
        {
            Log.Information("Building application");
        }

        SlabApplicationBuilder applicationBuilder = new SlabApplicationBuilder(builder);
        BuildApplication(applicationBuilder);
        
        builder.Services.AddQuartz(q =>
        {
            List<JobKey> jobKeys = new List<JobKey>();
            foreach ((Type type, JobKey jobKey, Action<ITriggerConfigurator> configurator) jobInfo in applicationBuilder.RegisteredJobs)
            {
                if (!jobKeys.Contains(jobInfo.jobKey))
                {
                    q.AddJob(jobInfo.type, jobInfo.jobKey);
                    jobKeys.Add(jobInfo.jobKey);
                }
                
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
        
        using (LogContext.PushProperty("SourceContext", GetType().FullName))
        {
            Log.Information("Setting up application");
        }

        SetupApplication(host);
        
        using (LogContext.PushProperty("SourceContext", GetType().FullName))
        {
            Log.Information("Running application");
        }

        host.Run();
    }
    
    protected abstract TBuilder CreateBuilder(string[]? args);
    
    protected abstract THost CreateHost(TBuilder builder);
    
    protected abstract void BuildApplication(ISlabApplicationBuilder appBuilder);
    
    protected abstract void SetupApplication(THost host);
}
