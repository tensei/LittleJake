using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleSteve.Data.Entities
{
    public class TwitchAlertSubscription
    {
        public int Id { get; set; }
        public long DiscordChannelId { get; set; }
        public TwitchStreamer TwitchStreamer { get; set; }
        public long TwitchStreamerId { get; set; }
    }
}
