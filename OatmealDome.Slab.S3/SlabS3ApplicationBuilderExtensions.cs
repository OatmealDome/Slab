namespace OatmealDome.Slab.S3;

public static class SlabS3ApplicationBuilderExtensions
{
    public static void RegisterS3(this ISlabApplicationBuilder builder)
    {
        builder.RegisterConfiguration<SlabS3Configuration>("S3");
        builder.RegisterSingleton<SlabS3Service>();
    }
}
