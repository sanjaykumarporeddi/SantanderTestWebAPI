using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SantanderTestWebAPI.Interfaces;
using SantanderTestWebAPI.Models;

namespace SantanderTestWebAPI.Services
{
    public class HackerNewsFetcher : IHackerNewsFetcher
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        private readonly IMemoryCacheService _cacheService;

        public HackerNewsFetcher(HttpClient httpClient, IOptions<ApiSettings> options, IMemoryCacheService cacheService)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiSettings = options.Value ?? throw new ArgumentNullException(nameof(options));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        public async Task<IEnumerable<BestStory>> GetBestStoriesAsync(int count)
        {
            try
            {
                // Generate a unique cache key based on the count
                var cacheKey = $"{_apiSettings.CacheKey}_{count}";

                // Try to retrieve stories from cache
                var (isCached, cachedStories) = await _cacheService.TryGetAsync<List<BestStory>>(cacheKey);
                if (isCached)
                {
                    return cachedStories;
                }

                // If not found in cache, fetch stories from Hacker News API asynchronously
                var bestStoriesIds = await GetBestStoriesIdsAsync();
                var stories = new List<BestStory>();

                for (int i = 0; i < count; i++)
                {
                    if (i >= bestStoriesIds.Count)
                        break;

                    var storyId = bestStoriesIds[i];
                    BestStory story;

                    story = await GetOrAddStoryToCacheAsync(storyId);

                    stories.Add(story);
                }

                // Cache fetched stories with the unique cache key
                await _cacheService.SetAsync(cacheKey, stories, TimeSpan.FromMinutes(_apiSettings.CacheDurationMinutes));

                return stories;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving best stories.", ex);
            }
        }

        private async Task<List<int>> GetBestStoriesIdsAsync()
        {
            var response = await _httpClient.GetAsync(_apiSettings.BestStoriesUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<int>>(content);
        }

        private async Task<BestStory> GetOrAddStoryToCacheAsync(int storyId)
        {
            var cacheKey = $"{_apiSettings.CacheKey}_{storyId}";

            var (isCached, cachedStory) = await _cacheService.TryGetAsync<BestStory>(cacheKey);
            if (isCached)
            {
                return cachedStory;
            }

            var storyDetails = await GetStoryDetailsAsync(storyId);
            var story = new BestStory()
            {
                Title = storyDetails.Title,
                Uri = storyDetails.Url,
                PostedBy = storyDetails.PostedBy,
                Time = storyDetails.Time,
                Score = storyDetails.Score,
                CommentCount = storyDetails.Descendants
            };

            await _cacheService.SetAsync(cacheKey, story, TimeSpan.FromMinutes(_apiSettings.CacheDurationMinutes));

            return story;
        }

        private async Task<StoryDetails> GetStoryDetailsAsync(int storyId)
        {
            var response = await _httpClient.GetAsync(string.Format(_apiSettings.StoryDetailsUrl, storyId));
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StoryDetails>(content);
        }
    }
}
