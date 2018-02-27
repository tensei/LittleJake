namespace LittleSteve.Data.Entities
{
    public class GuildOwner
    {
        public long DiscordId { get; set; }
        public long GuildId { get; set; }
        public TwitterUser TwitterUser { get; set; }
        public long TwitterUserId { get; set; }
        public TwitchStreamer TwitchStreamer { get; set; }
        public long TwitchStreamerId { get; set; }
    }
}