# Best Practices

## AddDbContext vs AddDbContextPool

In a web application, when registering a DbContext via AddDbContext, a new DbContext is instantiated for each HTTP request.

Using DBContext pooling via AddDbContextPool, a new DbContext is not instantiated with each request. Instead, a pool of DbContexts is kept for reuse. The same DbContext may be used across HTTP requests but it is reset to its default state between requests.

In low volume situations, AddDbContext is the better choice as it allows DbContexts to be disposed of, freeing up resources. In high volume situations, AddDbContextPool allows DbContexts to be reused. This allows for better performance as fewer resources are dedicated to instantiating and disposing of DbContexts.

For applications with many concurring scopes (ex: web applications or applications with many concurrent, independent jobs), use AddDbContextPool. Otherwise use AddDbContext.

## HttpClient

HttpClient should not be instantiated inline because disposal of the HttpClient before the socket is released can lead to socket exhaustion in high-volume applications. It should also not be used as a singleton because it will not pick up DNS changes. This has often tripped up developers as there was no obvious way to use HttpClient correctly. In .NET Core, there is a way to use HttpClient that mitigates these issues.

In the component needing to send an HTTP request, take a dependency on HttpClient.

```cs
public class DependencyWithHttp
{
    public HttpClient HttpClient;

    public DependencyWithHttp(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }
}
```

Install the NuGet package Microsoft.Extensions.Http. This adds an AddHttpClient extension method to the service collection.

```cs
services.AddHttpClient<DependencyWithHttp>();
```

This way you can use HttpClient in your component without worrying about DNS stagnation or socket exhaustion.

## IServiceScopeFactory

It is sometimes necessary to create scopes from within a service. One example of this is an application that runs multiple, concurrent jobs which depend on a DbContext. Each job should have its own DbContext and any service in that job should use the same DbContext.

Instead of passing around the ServiceProvider to create a scope, have the service depend on an IServiceScopeFactory. In the following example, any DbContext or other service with a scoped lifetime will be instantiated for each job but the instance will be reused throughout the job.

```cs
public class ParallelJobRunner
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ParallelJobRunner(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task RunLotsOfParallelJobs()
    {
        await Task.WhenAll(
            Enumerable.Range(1, 100)
                      .Select(_ => RunASingleJob())
                      .ToArray());
    }

    private async Task RunASingleJob()
    {
        using(var scope = _serviceScopeFactory.CreateScope())
        {
            var job = scope.ServiceProvider.GetRequiredService<IJob>();

            await job.Run();
        }
    }
}
```
