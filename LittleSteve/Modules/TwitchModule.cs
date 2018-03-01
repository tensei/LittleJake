using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FluentScheduler;
using LittleSteve.Data;
using LittleSteve.Data.Entities;
using LittleSteve.Jobs;
using LittleSteve.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LittleSteve.Modules
{
    [Group("twitch")]
    public class TwitchModule : ModuleBase<SteveBotCommandContext>
    {
        private readonly SteveBotContext _botContext;
        private readonly TwitchService _twitchService;


        public TwitchModule(TwitchService twitchService, SteveBotContext botContext)
        {
            _twitchService = twitchService;
            _botContext = botContext;
        }

        [Command("add")]
        public async Task AddTwitch(string twitchName, IGuildChannel guildChannel)
        {
            var userResponse = await _twitchService.GetUserByNameAsync(twitchName);

            if (userResponse is null)
            {
                await ReplyAsync("User Not Found");
                return;
            }

            var user = await _botContext.TwitchStreamers.Include(x => x.TwitchAlertSubscriptions)
                .FirstOrDefaultAsync(x => x.Id == long.Parse(userResponse.Id));

            if (user is null)
            {
                user = new TwitchStreamer
                {
                    Id = long.Parse(userResponse.Id),
                    Name = userResponse.DisplayName,
                    TwitchAlertSubscriptions = new List<TwitchAlertSubscription>()
                };
                _botContext.TwitchStreamers.Add(user);
                JobManager.AddJob(
                    () => new TwitchMonitoringJob(user.Id, _twitchService,
                        Context.Provider.GetService<SteveBotContext>(), Context.Client).Execute(),
                    s => s.WithName(userResponse.DisplayName).ToRunEvery(60).Seconds());
            }

            if (user.TwitchAlertSubscriptions.Any(x => x.DiscordChannelId == (long) guildChannel.Id))
            {
                await ReplyAsync($"You already subscribed to {user.Name} in {guildChannel.Name}");
                return;
            }

            user.TwitchAlertSubscriptions.Add(new TwitchAlertSubscription
            {
                DiscordChannelId = (long) guildChannel.Id,
                TwitchStreamerId = user.Id
            });

            var changes = _botContext.SaveChanges();

            if (changes > 0)
            {
                await ReplyAsync($"Alert for {user.Name} added to {guildChannel.Name}");
            }
            else
            {
                await ReplyAsync($"Unable to create Alert for {user.Name}");
            }
        }
    }
}