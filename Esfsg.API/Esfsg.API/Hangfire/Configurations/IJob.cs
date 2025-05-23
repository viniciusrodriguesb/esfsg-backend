namespace Esfsg.API.Hangfire.Configurations
{
    public interface IJob
    {
        Task Execute();
    }
}
