using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LittleSteve.Data.Entities
{
    public class Youtuber
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset LatestVideoDate { get; set; }
        public ICollection<YoutubeAlertSubscription> YoutubeAlertSubscriptions { get; set; }
        [NotMapped] public string ChannelUrl => $"https://www.youtube.com/channel/{Id}";
    }
}