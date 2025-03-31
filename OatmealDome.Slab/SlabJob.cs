using Quartz;

namespace OatmealDome.Slab;

public abstract class SlabJob : IJob
{
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
