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
    public class TwitterModule : ModuleBase<SteveBotCommandContext>
    {
        private readonly SteveBotContext _botContext;
        private readonly BotConfig _config;
        private readonly TwitterService _twitterService;

        public TwitterModule(TwitterService twitterService, SteveBotContext botContext, BotConfig config)
        {
            _twitterService = twitterService;
            _botContext = botContext;
            _config = config;
        }

        [Command("add")]
        [RequireOwnerOrAdmin]
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
                        Context.Provider.GetService<SteveBotContext>(), Context.Client).Execute(),
                    s => s.WithName(userResponse.ScreenName).ToRunEvery(30).Seconds());
            }

            if (user.TwitterAlertSubscriptions.Any(x => x.DiscordChannelId == (long) guildChannel.Id))
            {
                await ReplyAsync($"You already subscribed to {user.ScreenName} in {guildChannel.Name}");
                return;
            }

            user.TwitterAlertSubscriptions.Add(new TwitterAlertSubscription
            {
                DiscordChannelId = (long) guildChannel.Id,
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

        protected override void AfterExecute(CommandInfo command)
        {
            _botContext.Dispose();
        }
    }
}