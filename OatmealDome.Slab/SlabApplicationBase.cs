namespace OatmealDome.Slab;

public abstract class SlabApplicationBase
{
    internal string? EnvironmentName
    {
        get;
        set;
    }

    internal SlabApplicationBase()
    {
        //
    }
    
    internal abstract void Run(string[]? args);
}
