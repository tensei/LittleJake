namespace LittleSteve.Data.Entities
{
    public class TwitterAlert
    {
        public int Id { get; set; }
        public ulong DiscordChannelId { get; set; }
        public TwitterUser User { get; set; }
        public long TwitterUserId { get; set; }
    }
}