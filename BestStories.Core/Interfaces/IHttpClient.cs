namespace BestStories.Core.Interfaces
{
    public interface IHttpClient
    {
        Task<T> Get<T>(string uri);
    }
}
