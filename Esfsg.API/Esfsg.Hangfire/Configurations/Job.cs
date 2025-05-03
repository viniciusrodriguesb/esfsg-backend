namespace Esfsg.Hangfire.Configurations
{
    public abstract class Job : IJob
    {
        public abstract Task Execute();
    }
}
