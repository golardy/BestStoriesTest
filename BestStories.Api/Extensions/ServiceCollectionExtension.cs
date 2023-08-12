using BestStories.Core.Implementations;
using BestStories.Core.Interfaces;

namespace BestStories.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IBestStoriesService, BestStoriesService>();
            services.AddTransient<IHttpClient, Core.Implementations.HttpClient>();

            return services;
        }
    }
}
