namespace OatmealDome.Slab.TestConsoleApp;

internal sealed class TestConfiguration
{
    public string Key
    {
        get;
        set;
    } = "DefaultValue";

    public string OverriddenByLocalKey
    {
        get;
        set;
    } = "DefaultValueBySource";
}
