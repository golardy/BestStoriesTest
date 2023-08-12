using BestStories.Core.Interfaces;
using System.Net.Http.Json;

namespace BestStories.Core.Implementations
{
    /// <summary>
    /// Class responsible for providing http client inetraction with end points
    /// </summary>
    public class HttpClient : IHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<T> Get<T>(string uri)
        {
            var httpClient = _httpClientFactory.CreateClient("BestStoriesClient");

            return await httpClient.GetFromJsonAsync<T>(uri);
        }
    }
}
