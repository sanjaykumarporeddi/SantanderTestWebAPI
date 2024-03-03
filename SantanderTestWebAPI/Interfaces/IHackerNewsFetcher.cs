using System.Collections.Generic;
using System.Threading.Tasks;
using SantanderTestWebAPI.Models;

namespace SantanderTestWebAPI.Interfaces
{
    public interface IHackerNewsFetcher
    {
        Task<IEnumerable<BestStory>> GetBestStoriesAsync(int count);
    }
}
