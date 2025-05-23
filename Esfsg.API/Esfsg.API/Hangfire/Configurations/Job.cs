namespace Esfsg.API.Hangfire.Configurations
{
    public abstract class Job : IJob
    {
        public abstract Task Execute();
    }
}
