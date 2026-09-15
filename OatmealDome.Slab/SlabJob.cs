using Quartz;

namespace OatmealDome.Slab;

public abstract class SlabJob : IJob
{
    public ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken)
    {
        try
        {
            return new ValueTask(Run(context));
        }
        catch (Exception e)
        {
            throw new JobExecutionException(e);
        }
    }
    
    protected abstract Task Run(IJobExecutionContext context);
}
