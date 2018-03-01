using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CodeHollow.FeedReader.Feeds;
using LittleSteve.Models;

namespace LittleSteve.Services
{
    //maybe make this implement IFeedReader or something :thinking:
    public static class YoutubeFeedReader
    {
        private static readonly string _youtubeFeedUrl = "https://www.youtube.com/feeds/videos.xml?channel_id=";
        public static async Task<YoutubeFeed> ReadAsync(string youtubeChannelId)
        {
            try
            {
                var feedUrl = $"{_youtubeFeedUrl}{youtubeChannelId}";
                var feed = await CodeHollow.FeedReader.FeedReader.ReadAsync(feedUrl);
                var videos = feed.Items.Select(x => new YoutubeVideo() { Id = x.Id, DatePublished = DateTimeOffset.Parse(x.PublishingDateString), Url = x.Link, User = x.Author }).ToList();


                return new YoutubeFeed() { YoutubeVideos = videos };
            }
            catch (XmlException e)
            {
                return new YoutubeFeed(){YoutubeVideos = new List<YoutubeVideo>()};
            }
           
        }

        public static async Task<YoutubeVideo> GetLastestVideoFromFeed(string youtubeChannelId)
        {
            
            var feed = await ReadAsync(youtubeChannelId);
            return feed.YoutubeVideos.FirstOrDefault();
        }

    }
}
