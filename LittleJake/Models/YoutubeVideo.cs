using System;

namespace LittleJake.Models
{
    public class YoutubeVideo
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public DateTimeOffset DatePublished { get; set; }
        public string User { get; set; }
    }
}