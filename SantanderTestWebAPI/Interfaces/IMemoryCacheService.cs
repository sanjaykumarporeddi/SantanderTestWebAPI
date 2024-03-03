using System;
using System.Threading.Tasks;

namespace SantanderTestWebAPI.Interfaces
{
    public interface IMemoryCacheService
    {
        Task<(bool, T)> TryGetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan expiration);
    }
}
