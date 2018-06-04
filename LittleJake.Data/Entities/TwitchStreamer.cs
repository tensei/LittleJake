using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LittleJake.Data.Entities
{
    public class TwitchStreamer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        // typo ignore
        public DateTimeOffset SteamStartTime { get; set; }
        public DateTimeOffset StreamEndTime { get; set; }

        [NotMapped] public TimeSpan StreamLength => StreamEndTime - SteamStartTime;
        public ICollection<Game> Games { get; set; }
        public ICollection<TwitchAlertSubscription> TwitchAlertSubscriptions { get; set; }
    }
}