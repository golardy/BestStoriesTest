using AutoMapper;
using BestStories.Api.Models;
using BestStories.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BestStories.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BestStoriesController : ControllerBase
    {
        private readonly IBestStoriesService _bestStoriesRetriever;
        private readonly IMapper _mapper;

        public BestStoriesController(IBestStoriesService bestStoriesRetriever,
            IMapper mapper)
        {
            _bestStoriesRetriever = bestStoriesRetriever;
            _mapper = mapper;
        }

        [HttpGet("{count}")]
        public async Task<IEnumerable<StoryItem>> Get(int count)
        {
            var result = await _bestStoriesRetriever.Get(count);

            return _mapper.Map<IEnumerable<StoryItem>>(result);
        }
    }
}
