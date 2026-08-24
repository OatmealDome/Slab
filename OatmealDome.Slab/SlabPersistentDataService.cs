using System.Reflection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OatmealDome.Slab;

public sealed class SlabPersistentDataService
{
    private readonly ILogger<SlabPersistentDataService> _logger;
    private readonly SlabPersistentDataConfiguration _settings;

    private readonly string _root;

    public SlabPersistentDataService(ILogger<SlabPersistentDataService> logger,
        IOptions<SlabPersistentDataConfiguration> options, IHostEnvironment environment)
    {
        _logger = logger;
        _settings = options.Value;

        if (string.IsNullOrWhiteSpace(_settings.Root))
        {
            _root = Directory.GetCurrentDirectory();
        }
        else
        {
            _root = _settings.Root;
        }
    }

    public string GetPath(string file)
    {
        return Path.Combine(_root, file);
    }
}
