﻿namespace BestStories.Core.Models.Response
{
    public class StoryItemResponse
    {
        public string? Title { get; set; }
        public string? Url { get; set; }
        public string? By { get; set; }
        public long Time { get; set; }
        public int Score { get; set; }
        public int Descendants { get; set; }
    }
}
