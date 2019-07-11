namespace RecurringConsoleApplication.Dependencies
{
    public class Dependency
    {
        public readonly MyDbContext MyDbContext;
        public readonly ChildDependency ChildDependency;
        public readonly Logger Logger;

        public Dependency(MyDbContext myDbContext, ChildDependency childDependency, Logger logger)
        {
            MyDbContext = myDbContext;
            ChildDependency = childDependency;
            Logger = logger;
        }
    }
}
