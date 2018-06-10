using System;
using System.Linq;
using Discord.WebSocket;
using FluentScheduler;
using LittleJake.Data;
using LittleJake.Extensions;
using LittleJake.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LittleJake.Jobs
{
    public class YoutubeMonitoringJob : IJob
    {
        private readonly JakeBotContext _botContext;
        private readonly string _channelId;

        private readonly DiscordSocketClient _client;

        // private readonly string _feedUrl;
        public YoutubeMonitoringJob(string channelId, JakeBotContext botContext, DiscordSocketClient client)
        {
            _channelId = channelId;
            _botContext = botContext;
            _client = client;
            // _feedUrl = $@"https://www.youtube.com/feeds/videos.xml?channel_id={_channelId}";
        }

        public void Execute()
        {
            var youtuber = _botContext.Youtubers.Include(x => x.YoutubeAlertSubscriptions)
                .FirstOrDefault(x => x.Id == _channelId);
            if (youtuber == null)
            {
                Log.Information($"Error getting Youtuber for {_channelId}");
                return;
            }

            var video = YoutubeFeedReader.GetLastestVideoFromFeed(_channelId).AsSync(false);
            if (video == null)
            {
                Log.Information($"Error getting videos for {youtuber.Name}");
                return;
            }

            if (video.DatePublished - youtuber.LatestVideoDate <= TimeSpan.Zero)
            {
                return;
            }

            youtuber.LatestVideoDate = video.DatePublished;

            foreach (var subscription in youtuber.YoutubeAlertSubscriptions)
            {
                var channel = _client.GetChannel((ulong)subscription.DiscordChannelId) as SocketTextChannel;
                if (channel is null)
                {
                    Log.Information($"Youtube: {youtuber.Name} missing channel {subscription.DiscordChannelId} on server {channel.Guild.Name}");
                    continue;
                }
                channel.SendMessageAsync(video.Url);
            }

            _botContext.SaveChanges();
        }
    }
}