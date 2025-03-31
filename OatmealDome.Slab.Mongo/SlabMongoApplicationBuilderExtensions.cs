namespace OatmealDome.Slab.Mongo;

public static class SlabMongoApplicationBuilderExtensions
{
    public static void RegisterMongo<T>(this ISlabApplicationBuilder builder, string sectionName = "Mongo")
        where T : SlabMongoService
    {
        Type type = typeof(T);

        if (type == typeof(SlabMongoService))
        {
            throw new SlabException("Specified class must be subclass of SlabMongoService");
        }
        
        builder.RegisterConfiguration<SlabMongoConfiguration>(sectionName);
        builder.RegisterHostedService<T>();
    }
}
