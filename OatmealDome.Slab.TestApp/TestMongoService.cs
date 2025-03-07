using Microsoft.Extensions.Options;
using OatmealDome.Slab.Mongo;

namespace OatmealDome.Slab.TestApp;

public class TestMongoService : SlabMongoService
{
    public TestMongoService(IOptions<SlabMongoConfiguration> options) : base(options)
    {
        //
    }
}
