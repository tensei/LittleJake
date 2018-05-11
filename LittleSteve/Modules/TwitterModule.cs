using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FluentScheduler;
using LittleSteve.Data;
using LittleSteve.Data.Entities;
using LittleSteve.Jobs;
using LittleSteve.Models;
using LittleSteve.Preconditions;
using LittleSteve.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LittleSteve.Modules
{
    [Group("twitter")]
    [Name("Twitter")]
    [RequireContext(ContextType.Guild)]
    public class TwitterModule : ModuleBase<JakeBotCommandContext>
    {
        private readonly JakeBotContext _botContext;
        private readonly BotConfig _config;
        private readonly IServiceProvider _provider;
        private readonly TwitterService _twitterService;

        public TwitterModule(TwitterService twitterService, JakeBotContext botContext, BotConfig config,
            IServiceProvider provider)
        {
            _twitterService = twitterService;
            _botContext = botContext;
            _config = config;
            _provider = provider;
        }

        [Command]
        [Summary("Get the latest status from the default twitter")]
        [Blacklist]
        [ThrottleCommand]
        [Remarks("?twitter")]
        public async Task Twitter()
        {
            if (Context.GuildOwner is null || Context.GuildOwner.TwitterUserId == 0)
            {
                return;
            }

            var tweet = await _twitterService.GetLatestTweetForUserAsync(Context.GuildOwner.TwitterUserId);

            await ReplyAsync($@"https://twitter.com/{tweet.User.ScreenName}/status/{tweet.Id}");
        }

        [Command("add")]
        [RequireOwnerOrAdmin]
        [Summary("Add twitter account to follow in a specified channel")]
        [Remarks("?twitter add jakenbakelive #jakenbakelivehub")]
        public async Task AddTwitter(string twitterName, IGuildChannel guildChannel)
        {
            var userResponse = await _twitterService.GetUserFromHandle(twitterName);


            if (userResponse?.Id is null)
            {
                await ReplyAsync("User Not Found");
                return;
            }

            var user = await _botContext.TwitterUsers.Include(x => x.TwitterAlertSubscriptions)
                .FirstOrDefaultAsync(x => x.Id == userResponse.Id);
            if (user is null)
            {
                user = new TwitterUser
                {
                    Id = userResponse.Id.Value,
                    Name = userResponse.Name,
                    TwitterAlertSubscriptions = new List<TwitterAlertSubscription>(),
                    ScreenName = userResponse.ScreenName
                };
                _botContext.TwitterUsers.Add(user);
                JobManager.AddJob(
                    () => new TwitterMonitoringJob(user.Id, _twitterService,
                        _provider.GetService<JakeBotContext>(), Context.Client).Execute(),
                    s => s.WithName($"Twitter: {userResponse.ScreenName}").ToRunEvery(30).Seconds());
            }

            if (user.TwitterAlertSubscriptions.Any(x => x.DiscordChannelId == (long)guildChannel.Id))
            {
                await ReplyAsync($"You already subscribed to {user.ScreenName} in {guildChannel.Name}");
                return;
            }

            user.TwitterAlertSubscriptions.Add(new TwitterAlertSubscription
            {
                DiscordChannelId = (long)guildChannel.Id,
                TwitterUserId = user.Id
            });

            var changes = _botContext.SaveChanges();

            if (changes > 0)
            {
                await ReplyAsync($"Alert for {user.ScreenName} added to {guildChannel.Name}");
            }
            else
            {
                await ReplyAsync($"Unable to create Alert for {user.ScreenName}");
            }
        }

        [Command("remove")]
        [RequireOwnerOrAdmin]
        [Summary("Remove twitter account follow in a specified channel")]
        [Remarks("?twitter remove jakenbakelive #jakenbakelivehub")]
        public async Task RemoveTwitter(string twitterName, IGuildChannel guildChannel)
        {
            var twitter = await _botContext.TwitterUsers.Include(x => x.TwitterAlertSubscriptions).FirstOrDefaultAsync(
                x =>
                    x.ScreenName.Equals(twitterName, StringComparison.CurrentCultureIgnoreCase));

            if (twitter is null)
            {
                await ReplyAsync("Twitter Not Found");
                return;
            }

            var alert = twitter.TwitterAlertSubscriptions.FirstOrDefault(x =>
                x.DiscordChannelId == (long)guildChannel.Id);
            if (alert is null)
            {
                await ReplyAsync($"This channel doesnt contain an alert for {twitter.ScreenName}");
                return;
            }

            twitter.TwitterAlertSubscriptions.Remove(alert);
            if (Context.GuildOwner.TwitterUserId == twitter.Id)
            {
                var owner = await _botContext.GuildOwners.FindAsync(Context.GuildOwner.DiscordId,
                    Context.GuildOwner.GuildId);
                owner.TwitterUserId = 0;
            }

            if (!twitter.TwitterAlertSubscriptions.Any())
            {
                JobManager.RemoveJob($"Twitter: {twitter.Name}");
                _botContext.TwitterUsers.Remove(twitter);
            }

            var changes = _botContext.SaveChanges();

            if (changes > 0)
            {
                await ReplyAsync($"Alert for {twitter.ScreenName} removed from {guildChannel.Name}");
            }
            else
            {
                await ReplyAsync($"Unable to remove Alert for {twitter.ScreenName}");
            }
        }

        protected override void AfterExecute(CommandInfo command)
        {
            _botContext.Dispose();
        }
    }
}