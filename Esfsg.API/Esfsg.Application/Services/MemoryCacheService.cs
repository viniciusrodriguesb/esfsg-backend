using Esfsg.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Esfsg.Application.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {

        private readonly IMemoryCache _cache;
        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key)
        {
            _cache.TryGetValue(key, out T value);
            return value;
        }

        public void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null)
        {
            var options = new MemoryCacheEntryOptions();

            if (absoluteExpiration.HasValue)
                options.SetAbsoluteExpiration(absoluteExpiration.Value);

            _cache.Set(key, value, options);
        }
    }
}
