namespace Esfsg.Hangfire.Configurations
{
    public interface IJob
    {
        Task Execute();
    }
}
