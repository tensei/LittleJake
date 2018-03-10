using System;
using System.Collections.Generic;

namespace LittleSteve.Data.Entities
{
    public class Youtuber
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset LatestVideoDate { get; set; }
        public ICollection<YoutubeAlertSubscription> YoutubeAlertSubscriptions { get; set; }
       
    }
}