namespace LittleSteve.Data.Entities
{
    public class GuildOwner
    {
        public long DiscordId { get; set; }
        public long GuildId { get; set; }
        public long TwitterUserId { get; set; }
        public long TwitchStreamerId { get; set; }
        public string YoutuberId { get; set; }
    }
}