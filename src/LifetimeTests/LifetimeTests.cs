using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LifetimeTests
{
    public class LifetimeTests
    {
        [Fact]
        public void Transient()
        {
            var services = new ServiceCollection();

            services.AddTransient<Dependency>();

            var provider = services.BuildServiceProvider();

            var dependency1 = provider.GetRequiredService<Dependency>();
            var dependency2 = provider.GetRequiredService<Dependency>();

            dependency1.ShouldNotBeSameAs(dependency2);
        }

        [Fact]
        public void Singleton()
        {
            var services = new ServiceCollection();

            services.AddSingleton<Dependency>();

            var provider = services.BuildServiceProvider();

            var dependency1 = provider.GetRequiredService<Dependency>();
            var dependency2 = provider.GetRequiredService<Dependency>();

            dependency1.ShouldBeSameAs(dependency2);
        }

        [Fact]
        public void Scoped()
        {
            var services = new ServiceCollection();

            services.AddScoped<Dependency>();

            var provider = services.BuildServiceProvider();

            var parentDependency1 = provider.GetRequiredService<Dependency>();
            var parentDependency2 = provider.GetRequiredService<Dependency>();

            Dependency scopedDependency1;
            Dependency scopedDependency2;
            using (var scope = provider.CreateScope())
            {
                scopedDependency1 = scope.ServiceProvider.GetRequiredService<Dependency>();
                scopedDependency2 = scope.ServiceProvider.GetRequiredService<Dependency>();
            }            

            this.ShouldSatisfyAllConditions(
                () => parentDependency1.ShouldNotBeSameAs(scopedDependency1),
                () => scopedDependency1.ShouldBeSameAs(scopedDependency2),
                () => scopedDependency1.ShouldNotBeSameAs(parentDependency2),
                () => parentDependency1.ShouldBeSameAs(parentDependency2)
            );            
        }

        [Fact]
        public void SingletonInAScope()
        {
            var services = new ServiceCollection();

            services.AddSingleton<Dependency>();

            var provider = services.BuildServiceProvider();

            var dependency1 = provider.GetRequiredService<Dependency>();

            Dependency dependency2;
            using (var scope = provider.CreateScope())
            {
                dependency2 = scope.ServiceProvider.GetRequiredService<Dependency>();
            }

            dependency1.ShouldBeSameAs(dependency2);
        }

        [Fact]
        public void HttpClientFactory()
        {
            var services = new ServiceCollection();

            services.AddTransient<DependencyWithHttp>();
            services.AddHttpClient<DependencyWithHttp>();

            var provider = services.BuildServiceProvider();

            var dependency = provider.GetRequiredService<DependencyWithHttp>();

            dependency.HttpClient.ShouldNotBeNull();
        }
    }

    public class ParallelJobRunner
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ParallelJobRunner(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task RunLotsOfParallelJobs()
        {
            await Task.WhenAll(Enumerable.Range(1, 100)
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

    public interface IJob
    {
        Task Run();
    }
}
