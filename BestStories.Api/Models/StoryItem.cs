namespace BestStories.Api.Models
{
    public class StoryItem
    {
        /// <summary>
        /// Title value
        /// </summary>
        public required string Title { get; set; }
        /// <summary>
        /// Url value
        /// </summary>
        public required string Url { get; set; }
        /// <summary>
        /// Posted by value
        /// </summary>
        public required string By { get; set; }
        /// <summary>
        /// Posted time value
        /// </summary>
        public required long Time { get; set; }
        /// <summary>
        /// Score value
        /// </summary>
        public required int Score { get; set; }
        /// <summary>
        /// Descendants count
        /// </summary>
        public required int Descendants { get; set; }
    }
}
