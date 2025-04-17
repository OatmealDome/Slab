using System.Diagnostics;
using Serilog;
using Serilog.Context;
using Serilog.Sinks.SystemConsole.Themes;

namespace OatmealDome.Slab;

public static class SlabEntryPoint
{
    private const string LogFormat =
        "[{Timestamp:MM-dd-yyyy HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}";
    
    public static void RunApplication<T>(string[]? args) where T : SlabApplicationBase, new()
    {
        // Configure Serilog now. We do this as early in the start up process to ensure logging is always available.
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Information()
            .WriteTo.Async(c => c.Console(outputTemplate: LogFormat, theme: AnsiConsoleTheme.Sixteen))
            .WriteTo.Async(c =>
                c.File("Logs/.log", outputTemplate: LogFormat, rollingInterval: RollingInterval.Day))
            .CreateLogger();

        using (LogContext.PushProperty("SourceContext", typeof(SlabEntryPoint).FullName))
        {
            Log.Information("Entering RunApplication");
        }
        
#if DEBUG
        DateTime startTime = DateTime.UtcNow;

        while (!Debugger.IsAttached)
        {
            Thread.Sleep(100);
            
            TimeSpan difference = DateTime.UtcNow.Subtract(startTime);

            // If the debugger is taking too long to attach, just give up and proceed to start the application.
            if (difference.TotalSeconds > 5)
            {
                break;
            }
        }

        // Enforce an additional sleep to ensure that the debugger is fully initialized.
        if (Debugger.IsAttached)
        {
            Thread.Sleep(1000);
        }
#endif
        
        Type appType = typeof(T);
        T app = (T)Activator.CreateInstance(appType)!;

        bool failure = false;

        try
        {
            app.Run(args);
        }
        catch (Exception e)
        {
            failure = true;
            
            using (LogContext.PushProperty("SourceContext", typeof(SlabEntryPoint).FullName))
            {
                Log.Error(e, "Unhandled exception in application");
            }
        }
        
        Log.CloseAndFlush();
        
        Environment.Exit(failure ? 1 : 0);
    }
}
