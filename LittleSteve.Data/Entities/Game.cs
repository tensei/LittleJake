using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace LittleSteve.Data.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public string Name { get; set; }
        [NotMapped] public TimeSpan PlayLength => EndTime - StartTime;
        public TwitchStreamer TwitchStreamer { get; set; }
        public long TwitchStreamerId { get; set; }
    }
}
