public class ScopedJobFactory : IJobFactory
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ConcurrentDictionary<IJob, IServiceScope> _scopes = new ConcurrentDictionary<IJob, IServiceScope>();

    public ScopedJobFactory(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        var scope = _serviceScopeFactory.CreateScope();
        
        IJob job;
        try
        {
            job = (IJob)scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType);
        }
        catch
        {
            scope.Dispose();
            throw;
        }

        if (!_scopes.TryAdd(job, scope))
        {
            scope.Dispose();
            throw new Exception("Error storing job lifetime scope");
        }

        return job;
    }

    public void ReturnJob(IJob job)
    {
        if (_scopes.TryRemove(job, out var scope))
        {
            scope.Dispose();
        }
    }
}
