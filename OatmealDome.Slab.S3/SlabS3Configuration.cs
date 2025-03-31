namespace OatmealDome.Slab.S3;

public sealed class SlabS3Configuration
{
    public string ServiceUrl
    {
        get;
        set;
    } = string.Empty;

    public string Bucket
    {
        get;
        set;
    } = string.Empty;

    public string AccessKey
    {
        get;
        set;
    } = string.Empty;

    public string SecretAccessKey
    {
        get;
        set;
    } = string.Empty;
}
