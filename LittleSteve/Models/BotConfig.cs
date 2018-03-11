using Newtonsoft.Json;

namespace LittleSteve.Models
{
    public class BotConfig
    {
        public TwitterTokens TwitterTokens { get; set; }
        public string ConnectionString { get; set; }

        [JsonProperty("YoutubeKey")] public string YoutubeKey { get; set; }

        [JsonProperty("TwitchClientId")] public string TwitchClientId { get; set; }

        [JsonProperty("DiscordToken")] public string DiscordToken { get; set; }

        [JsonProperty("ImgurClientId")] public string ImgurClientId { get; set; }

        [JsonProperty("DestinyCalendar")] public string DestinyCalendar { get; set; }

        [JsonProperty("ImgurAlbumId")] public string ImgurAlbumId { get; set; }

        public string Prefix { get; set; }
        public int ThrottleLength { get; set; }
    }
}