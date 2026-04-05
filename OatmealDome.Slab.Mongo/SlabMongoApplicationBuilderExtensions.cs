using Microsoft.Extensions.DependencyInjection;

namespace OatmealDome.Slab.Mongo;

public static class SlabMongoApplicationBuilderExtensions
{
    public static void RegisterMongo(this ISlabApplicationBuilder builder,
        Action<ISlabMongoBuilder> serviceConfigurator, string sectionName = "Mongo")
    {
        SlabMongoBuilder mongoBuilder = new SlabMongoBuilder();
        serviceConfigurator(mongoBuilder);
        
        builder.RegisterConfiguration<SlabMongoConfiguration>(sectionName);
        builder.Services.AddSingleton(mongoBuilder.Registry);
        builder.RegisterHostedService<SlabMongoService>();
    }
}
