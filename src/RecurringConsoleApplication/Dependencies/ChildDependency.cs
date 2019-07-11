namespace RecurringConsoleApplication.Dependencies
{
    public class ChildDependency
    {
        public readonly Logger Logger;

        public ChildDependency(Logger logger)
        {
            Logger = logger;
        }
    }
}
