namespace LittleSteve.Data.Entities
{
    public class GuildOwner
    {
        public ulong DiscordId { get; set; }
        public ulong GuildId { get; set; }
        public TwitterUser TwitterUser { get; set; }
        public long TwitterUserId { get; set; }

    }
}
