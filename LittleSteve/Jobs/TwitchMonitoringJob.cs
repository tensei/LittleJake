using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using FluentScheduler;
using Humanizer;
using LittleSteve.Data;
using LittleSteve.Data.Entities;
using LittleSteve.Extensions;
using LittleSteve.Services;
using Microsoft.EntityFrameworkCore;
using TwitchLib.Models.API.v5.Streams;

namespace LittleSteve.Jobs
{
    public class TwitchMonitoringJob : IJob
    {
        private readonly long _channelId;
        private readonly TwitchService _twitchService;
        private readonly SteveBotContext _botContext;
        private readonly DiscordSocketClient _client;

        public TwitchMonitoringJob(long channelId,TwitchService twitchService,SteveBotContext botContext,DiscordSocketClient client)
        {
            _channelId = channelId;
            _twitchService = twitchService;
            _botContext = botContext;
            _client = client;
        }
        public void Execute()
        {
            using (_botContext)
            {
                var streamer = _botContext.TwitchStreamers.Include(x => x.TwitchAlertSubscriptions).FirstOrDefault(x => x.Id == _channelId);
                if (streamer is null)
                {
                    return;
                }
                var isStreaming = _twitchService.IsUserStreamingAsync(_channelId).AsSync(false);

                if (DateTimeOffset.UtcNow - streamer.StreamEndTime < TimeSpan.FromMinutes(5))
                {
                    return;
                }
                //stream has ended and we are waiting for startup
                if (!isStreaming && streamer.StreamLength >= TimeSpan.Zero )
                {
                    Console.WriteLine("has ended");
                    return;
                }
                // stream started
                if (isStreaming && streamer.StreamLength >= TimeSpan.Zero)
                {
                    var stream = _twitchService.GetStream(_channelId).AsSync(false);

                    streamer.SteamStartTime = stream.CreatedAt.ToUniversalTime();
                    foreach (var subscription in streamer.TwitchAlertSubscriptions)
                    {
                        var channel = _client.GetChannel((ulong)subscription.DiscordChannelId) as ITextChannel;
                        var message = channel.SendMessageAsync(string.Empty, embed: CreateTwitchEmbed(streamer, stream)).AsSync(false);
                        subscription.MessageId = (long) message.Id;
                        
                    }
                    _botContext.SaveChanges();
                    return;

                }
                // stream has started and we update the message embed
                if (isStreaming && streamer.StreamLength <= TimeSpan.Zero)
                {
                    var stream = _twitchService.GetStream(_channelId).AsSync(false);
                    if (stream is null)
                    {
                        return;
                    }
                    foreach (var subscription in streamer.TwitchAlertSubscriptions)
                    {
                        
                        var channel = _client.GetChannel((ulong)subscription.DiscordChannelId) as ITextChannel;

                        if (!(channel.GetMessageAsync((ulong) subscription.MessageId).AsSync(false) is RestUserMessage message))
                        {
                            return;
                        }
                        message.ModifyAsync(x => x.Embed = CreateTwitchEmbed(streamer, stream)).AsSync(false);

                    }
                    Console.WriteLine("update this");
                }
                //stream ended
                if (!isStreaming && streamer.StreamLength <= TimeSpan.Zero)
                {
                    var user = _twitchService.GetUserByIdAsync(streamer.Id).AsSync(false);
                    streamer.StreamEndTime = DateTimeOffset.UtcNow;
                    var description = new StringBuilder();
                    description.AppendLine($"**Started at:** {streamer.SteamStartTime:g} UTC");
                    description.AppendLine($"**Ended at:** {streamer.StreamEndTime:g} UTC");
                    description.AppendLine(String.Empty);
                    description.AppendLine($"**Total Time:** {streamer.StreamLength.Humanize(3)}");


                    var embed = new EmbedBuilder()
                        .WithAuthor($"{streamer.Name} was live", url: $"https://twitch.tv/{streamer.Name}")
                        .WithThumbnailUrl(user.Logo)
                        .WithDescription(description.ToString())
                        .Build();

                    foreach (var subscription in streamer.TwitchAlertSubscriptions)
                    {

                        var channel = _client.GetChannel((ulong)subscription.DiscordChannelId) as ITextChannel;

                        if (!(channel.GetMessageAsync((ulong)subscription.MessageId).AsSync(false) is RestUserMessage message))
                        {
                            return;
                        }
                        message.ModifyAsync(x => x.Embed =embed).AsSync(false);

                    }

                    Console.WriteLine("ended");
                }

                _botContext.SaveChanges();

            }



        }

        private Embed CreateTwitchEmbed(TwitchStreamer streamer,Stream stream)
        {
            
            return new EmbedBuilder()
                
                .WithAuthor($"{streamer.Name} is live", url: $"https://twitch.tv/{streamer.Name}")
                .WithTitle($"{stream.Channel.Status}")
                .WithUrl($"https://twitch.tv/{streamer.Name}")
                .WithThumbnailUrl(stream.Channel.Logo)
                .AddField("Playing",stream.Game, true)
                .AddField("Viewers",stream.Viewers, true)
                //we add the timeseconds so the image wont be used from the cache 
                .WithImageUrl($"{stream.Preview.Template.Replace("{width}","1920").Replace("{height}","1080")}?{DateTimeOffset.Now.ToUnixTimeSeconds()}")
                .Build();
        }
    }
}
