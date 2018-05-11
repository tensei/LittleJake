namespace LittleJake.Data.Entities
{
    public class YoutubeAlertSubscription
    {
        public int Id { get; set; }
        public long DiscordChannelId { get; set; }
        public Youtuber Youtuber { get; set; }
        public string YoutuberId { get; set; }
        public long MessageId { get; set; }
    }
}