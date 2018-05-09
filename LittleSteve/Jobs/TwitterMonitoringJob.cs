using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using FluentScheduler;
using LittleSteve.Data;
using LittleSteve.Data.Entities;
using LittleSteve.Extensions;
using LittleSteve.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LittleSteve.Jobs
{
    public class TwitterMonitoringJob : IJob
    {
        private readonly SteveBotContext _botContext;
        private readonly DiscordSocketClient _client;
        private readonly TwitterService _twitterService;
        private readonly long _twitterUserId;

        public TwitterMonitoringJob(long twitterUserId, TwitterService twitterService, SteveBotContext botContext,
            DiscordSocketClient client)
        {
            _twitterUserId = twitterUserId;
            _twitterService = twitterService;
            _botContext = botContext;
            _client = client;
        }

        public void Execute()
        {
            var missingChannels = new List<TwitterAlertSubscription>();
            using (_botContext)
            {
                var user = _botContext.TwitterUsers.Include(x => x.TwitterAlertSubscriptions)
                    .FirstOrDefault(x => x.Id == _twitterUserId);
                if (user is null)
                {
                    return;
                }

                if (user.LastTweetId == 0)
                {

                    Log.Information($"reached -1");
                    var tweet = _twitterService.GetLatestTweetForUserAsync(_twitterUserId).AsSync(false);
                    foreach (var twitterAlert in user.TwitterAlertSubscriptions)
                    {
                        var channel = _client.GetChannel((ulong)twitterAlert.DiscordChannelId) as ITextChannel;
                        if (channel is null)
                        {
                            Log.Information($"{user.ScreenName} removing for channel {twitterAlert.DiscordChannelId}");
                            missingChannels.Add(twitterAlert);
                            continue;
                        }
                        channel.SendMessageAsync($@"https://twitter.com/{user.ScreenName}/status/{tweet.Id}")
                            .AsSync(false);
                    }

                    Log.Information("{date}: {tweet}", tweet.CreatedAt, tweet.FullText ?? tweet.Text);
                    user.LastTweetId = tweet.Id;
                }
                else
                {
                    var tweets = _twitterService.GetTweetsSinceAsync(user.Id, user.LastTweetId).AsSync(false).ToList();

                    if (!tweets.Any())
                    {
                        return;
                    }
                    foreach (var twitterAlert in user.TwitterAlertSubscriptions)
                    {
                        var channel = _client.GetChannel((ulong)twitterAlert.DiscordChannelId) as ITextChannel;
                        if (channel is null)
                        {
                            Log.Information($"{user.ScreenName} removing for channel {twitterAlert.DiscordChannelId}");
                            missingChannels.Add(twitterAlert);
                            continue;
                        }
                        foreach (var tweet in tweets)
                        {
                            channel.SendMessageAsync($@"https://twitter.com/{user.ScreenName}/status/{tweet.Id}")
                                .AsSync(false);
                            Log.Information("{date}: {tweet}", tweet.CreatedAt, tweet.FullText);
                        }
                    }
                    user.LastTweetId = tweets.Last().Id;

                    missingChannels.ForEach(m => user.TwitterAlertSubscriptions.Remove(m));
                }

                _botContext.SaveChanges();
            }
        }
    }
}