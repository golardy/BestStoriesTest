using BestStories.Core.Models.Response;

namespace BestStories.Core.Interfaces
{
    public interface IBestStoriesService
    {
        Task<IEnumerable<StoryItemResponse>> Get(int count);
    }
}
