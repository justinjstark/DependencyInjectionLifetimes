using RecurringConsoleApplication.Dependencies;
using System;
using System.Threading.Tasks;

namespace RecurringConsoleApplication
{
    public class Job
    {
        public readonly MyDbContext MyDbContext;
        public readonly Dependency Dependency;
        public readonly ChildDependency ChildDependency;
        public readonly Logger Logger;

        public Job(MyDbContext myDbContext, Dependency dependency, ChildDependency childDependency, Logger logger)
        {
            MyDbContext = myDbContext;
            Dependency = dependency;
            ChildDependency = childDependency;
            Logger = logger;
        }

        public async Task Run(Job previousJob)
        {
            await Console.Out.WriteLineAsync(DateTime.Now.ToString());

            if (previousJob != null)
            {
                if (ReferenceEquals(MyDbContext, previousJob.MyDbContext))
                {
                    Console.WriteLine("MyDbContext is the same as last run");
                }
                if (ReferenceEquals(Dependency, previousJob.Dependency))
                {
                    Console.WriteLine("Dependency is the same as last run");
                }
                if (ReferenceEquals(ChildDependency, previousJob.ChildDependency))
                {
                    Console.WriteLine("ChildDependency is the same as last run");
                }
                if (ReferenceEquals(Logger, previousJob.Logger))
                {
                    Console.WriteLine("Logger is the same as last run");
                }
            }

            if (ReferenceEquals(MyDbContext, Dependency.MyDbContext))
            {
                await Console.Out.WriteLineAsync("Job.MyDbContext and Job.Dependency.MyDbContext are the same");
            }
            if (ReferenceEquals(ChildDependency, Dependency.ChildDependency))
            {
                await Console.Out.WriteLineAsync("Job.ChildDependency and Job.Dependency.ChildDependency are the same");
            }
            if (ReferenceEquals(Logger, Dependency.Logger) && ReferenceEquals(Logger, Dependency.ChildDependency.Logger))
            {
                await Console.Out.WriteLineAsync("Job.Logger, Job.Dependency.Logger, and Job.Dependency.ChildDependency.Logger are the same");
            }

            await Console.Out.WriteLineAsync();

            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
