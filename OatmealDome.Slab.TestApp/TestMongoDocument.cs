using MongoDB.Bson;

namespace OatmealDome.Slab.TestApp;

public class TestMongoDocument
{
    public ObjectId _id
    {
        get;
        set;
    } = ObjectId.Empty;

    public string Key
    {
        get;
        set;
    } = "DefaultValue";
}
