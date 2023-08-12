using AutoMapper;
using BestStories.Api.Models;
using BestStories.Core.Models.Response;

namespace BestStories.Api.Mapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<StoryItemResponse, StoryItem>();
        }
    }
}
