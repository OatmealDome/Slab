using Quartz;

namespace OatmealDome.Slab;

public abstract class SlabJob : IJob
{
    public const string ApplicationJobKeyGroup = "application";
    
    public static JobKey CreateJobKey(string name, string group = ApplicationJobKeyGroup)
    {
        return new JobKey(name, group);
    }
    
    public Task Execute(IJobExecutionContext context)
    {
        try
        {
            return Run(context);
        }
        catch (Exception e)
        {
            throw new JobExecutionException(e);
        }
    }
    
    protected abstract Task Run(IJobExecutionContext context);
}
