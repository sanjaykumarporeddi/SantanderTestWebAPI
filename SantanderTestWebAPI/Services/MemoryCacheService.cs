using Microsoft.Extensions.Caching.Memory;
using SantanderTestWebAPI.Interfaces;
using System;
using System.Threading.Tasks;

namespace SantanderTestWebAPI.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<(bool, T)> TryGetAsync<T>(string key)
        {
            if (_cache.TryGetValue(key, out T value))
            {
                return (true, value);
            }
            return (false, default);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            _cache.Set(key, value, expiration);
        }
    }
}
