using System.Reflection;

namespace OatmealDome.Slab;

public sealed class SlabEmbeddedResourceService
{
    private readonly Assembly _appAssembly;

    public SlabEmbeddedResourceService(Assembly assembly)
    {
        _appAssembly = assembly;
    }

    private Stream GetEmbeddedResourceAsStream(string fileName)
    {
        string? resourceName = _appAssembly.GetManifestResourceNames().SingleOrDefault(s => s.EndsWith(fileName));
        if (resourceName == null)
        {
            throw new FileNotFoundException($"Embedded resource {fileName} not found");
        }

        return _appAssembly.GetManifestResourceStream(resourceName)!;
    }

    public byte[] GetEmbeddedResourceAsByteArray(string fileName)
    {
        using Stream stream = GetEmbeddedResourceAsStream(fileName);

        byte[] data = new byte[stream.Length];
        stream.ReadExactly(data, 0, data.Length);
        
        return data;
    }
    
    public string GetEmbeddedResourceAsString(string fileName)
    {
        using Stream stream = GetEmbeddedResourceAsStream(fileName);
        using StreamReader reader = new(stream);
        
        return reader.ReadToEnd();
    }
}
