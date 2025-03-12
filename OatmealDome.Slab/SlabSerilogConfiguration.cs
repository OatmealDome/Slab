namespace OatmealDome.Slab;

internal sealed class SlabSerilogConfiguration
{
    public const string DefaultSlackWebhookUrl = "https://example.com/post";

    public string SlackWebhookUrl
    {
        get;
        set;
    } = DefaultSlackWebhookUrl;
}
