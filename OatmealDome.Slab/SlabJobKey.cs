using Quartz;

namespace OatmealDome.Slab;

// Wrapper class to prevent users from creating their own JobKey instances.
public sealed class SlabJobKey
{
    private const string ApplicationJobKeyGroup = "Application";
    
    public JobKey QuartzJobKey
    {
        get;
    }
    
    private SlabJobKey(JobKey jobKey)
    {
        QuartzJobKey = jobKey;
    }
    
    public static SlabJobKey Create(string name, string group = ApplicationJobKeyGroup)
    {
        return new SlabJobKey(new JobKey(name, group));
    }
}
