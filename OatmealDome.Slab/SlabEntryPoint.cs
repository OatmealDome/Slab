namespace OatmealDome.Slab;

public static class SlabEntryPoint
{
    public static void RunApplication<T>(string[]? args) where T : SlabApplicationBase, new()
    {
        Type appType = typeof(T);
        T app = (T)Activator.CreateInstance(appType)!;
        
        app.Run(args);
    }
}
