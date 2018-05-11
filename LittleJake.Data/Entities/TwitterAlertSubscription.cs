namespace LittleJake.Data.Entities
{
    public class TwitterAlertSubscription
    {
        public int Id { get; set; }
        public long DiscordChannelId { get; set; }
        public TwitterUser User { get; set; }
        public long TwitterUserId { get; set; }
    }
}