using System.Diagnostics;

namespace OatmealDome.Slab;

public static class SlabEntryPoint
{
    public static void RunApplication<T>(string[]? args) where T : SlabApplicationBase, new()
    {
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
        
        app.Run(args);
    }
}
