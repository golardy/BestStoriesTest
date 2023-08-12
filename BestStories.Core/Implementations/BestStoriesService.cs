using BestStories.Core.Interfaces;
using BestStories.Core.Models.Response;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BestStories.Core.Implementations
{
    /// <summary>
    /// Service that incapsulate logic of retriving data, sorting it and generate result
    /// </summary>
    public class BestStoriesService : IBestStoriesService
    {
        private static readonly SemaphoreSlim semaphoreSlim = new(1);
        private static readonly MemoryCacheEntryOptions cacheExpirationOptions = new()
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(5)
        };

        private readonly IHttpClient _dataSource;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<BestStoriesService> _logger;

        public BestStoriesService(IHttpClient dataSource, IMemoryCache memoryCache, ILogger<BestStoriesService> logger)
        {
            _dataSource = dataSource;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<IEnumerable<StoryItemResponse>> Get(int count)
        {
            var result = new List<StoryItemResponse>();
            var bestStoriesIds = await GetBestStoriesIds();
            var storiesData = await GetStoriesData(bestStoriesIds);

            var takenStoriesIds = bestStoriesIds.Take(count);
            foreach (var storyId in takenStoriesIds)
            {
                result.Add(storiesData[storyId]);
            }

            return result;
        }

        private async Task<int[]> GetBestStoriesIds()
        {
            var bestStoriesCacheKey = "bestStoriesList";

            if (_memoryCache.TryGetValue(bestStoriesCacheKey, out int[] bestStoriesIds))
            {
                _logger.LogDebug($"Cache HIT: Found {bestStoriesCacheKey}");
                return bestStoriesIds;
            }

            await semaphoreSlim.WaitAsync();

            try
            {
                bestStoriesIds = await _dataSource.Get<int[]>("beststories.json");
                _memoryCache.Set(bestStoriesCacheKey, bestStoriesIds, cacheExpirationOptions);

                return bestStoriesIds;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private async Task<Dictionary<int, StoryItemResponse>> GetStoriesData(int[] bestStoriesIds)
        {
            var bestStoriesItems = new Dictionary<int, StoryItemResponse>();

            await semaphoreSlim.WaitAsync();
            try
            {
                foreach (var id in bestStoriesIds)
                {
                    var bestStoryCacheKey = $"bestStory:{id}";
                    if (!_memoryCache.TryGetValue(bestStoryCacheKey, out StoryItemResponse storyItemResponse))
                    {
                        _logger.LogDebug($"Cache HIT: Not found {bestStoryCacheKey}");
                        storyItemResponse = await _dataSource.Get<StoryItemResponse>($"item/{id}.json");
                        _memoryCache.Set(bestStoryCacheKey, storyItemResponse, cacheExpirationOptions);
                    }

                    bestStoriesItems.Add(id, storyItemResponse);
                }

                return bestStoriesItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
