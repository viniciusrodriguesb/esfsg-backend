namespace Esfsg.Application.Interfaces
{
    public interface IMemoryCacheService
    {
        T Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null);
    }
}
