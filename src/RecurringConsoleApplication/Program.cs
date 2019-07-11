using RecurringConsoleApplication.Dependencies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace RecurringConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddTransient<Job>();
            services.AddDbContext<MyDbContext>(); //[1] [2] //Scoped
            services.AddTransient<Dependency>();
            services.AddTransient<ChildDependency>();
            services.AddSingleton<Logger>();
            var serviceProvider = services.BuildServiceProvider();

            Job previousJob = null; //[3]

            while (true)
            {
                using (var scope = serviceProvider.CreateScope()) //[4]
                {
                    var job = scope.ServiceProvider.GetRequiredService<Job>();

                    job.Run(previousJob).GetAwaiter().GetResult(); //[5]

                    previousJob = job;
                }

                Thread.Sleep(TimeSpan.FromSeconds(5)); //[6]
            }
        }
    }

    /*
     * [1] The default service lifetime for AddDbContext is Scoped.
     * [2] There is no need to use DbContext pooling here. DbContext pooling is useful in
     *     web apps to reduce the creation and disposal of DbContexts since a new one is
     *     created per HTTP request.
     * [3] We only track the previous job for demonstration purposes in order to write
     *     to the console which dependencies are the same across job runs.
     * [4] If a scope needs to be created from within a dependency, make the class
     *     depend on IServiceScopeFactory and use it to create the scope. This is useful
     *     if your job fires multiple subjobs that need to run in parallel. (Remember:
     *     A DbContext represents a unit of work. Do not share it across parallel jobs.)
     * [5] The same as .Wait() except it unwraps the AggregateException.
     * [6] This puts 5 seconds between the end of the last run and the beginning of the
     *     next. Drift will occur. If you need a job to run at some exact interval, use a
     *     scheduler like Quartz.NET.
     * 
     * Singleton: Lasts the lifetime of the application. Useful for things like
     *   caching and logging that you want to keep the instance across all job runs.
     * Transient: A new service will be created for every use.
     * Scoped: A new service will be created for every scope (every job run in this
     *   example). But inside the scope, the same instance will be used.
     */
}
