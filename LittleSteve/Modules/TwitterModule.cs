using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly TwitterService _twitterService;
        private readonly SteveBotContext _botContext;
        private readonly BotConfig _config;

        public TwitterModule(TwitterService twitterService,SteveBotContext botContext,BotConfig config)
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
           
            var user = await _botContext.TwitterUsers.FirstOrDefaultAsync(x => x.Id == userResponse.Id);
            if (user is null)
            {
                user = new TwitterUser()
                {
                    Id = userResponse.Id.Value,
                    Name = userResponse.Name,
                    ScreenName = userResponse.ScreenName
                };
                _botContext.TwitterUsers.Add(user);
                JobManager.AddJob(() => new TwitterMonitoringJob(user.Id,_twitterService,Context.Provider.GetService<SteveBotContext>(),Context.Client).Execute(),s => s.WithName(userResponse.ScreenName).ToRunEvery(30).Seconds() );
                
            }

           
            _botContext.Add(new TwitterAlert()
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
