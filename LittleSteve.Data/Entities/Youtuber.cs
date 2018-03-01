using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleSteve.Data.Entities
{
    public class Youtuber
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset LatestVideoDate  { get; set; }
        public ICollection<YoutubeAlertSubscription> YoutubeAlertSubscriptions { get; set; }
        public ICollection<GuildOwner> GuildOwners { get; set; }
    }
}
