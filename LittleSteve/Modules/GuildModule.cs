using System;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using Discord.Commands;
using LittleSteve.Data;
using LittleSteve.Data.Entities;
using LittleSteve.Preconditions;
using Microsoft.EntityFrameworkCore;

namespace LittleSteve.Modules
{
    [RequireContext(ContextType.Guild)]
    [Group("guild")]
    [Name("Guild")]
    public class GuildModule : InteractiveBase<SteveBotCommandContext>
    {
        private readonly SteveBotContext _botContext;

        public GuildModule(SteveBotContext botContext)
        {
            _botContext = botContext;
        }

        [RequireOwnerOrAdmin]
        [Command("setup", RunMode = RunMode.Async)]
        [Summary("Setup guild with default social media accounts.")]
        [Remarks("Just follow the prompts people")]
        public async Task Setup()
        {
            await ReplyAsync($"Setting up Guild for {Context.Guild.Owner.Username}");
            var owner = new GuildOwner
            {
                DiscordId = (long) Context.Guild.OwnerId,
                GuildId = (long) Context.Guild.Id
            };
            await ReplyAsync(
                "Would you like to setup Social Media Notifications? If you do please add the accounts you want to follow before continuing.\n\n**Type Yes to continue or Literally anything else to skip this**");
            var message = await NextMessageAsync(timeout: TimeSpan.FromSeconds(20));
            if (!message.Content.Equals("yes", StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }

            await ReplyAsync("Enter the name for the default Twitter account to follow");
            message = await NextMessageAsync();

            if (!(message is null))
            {
                var twitter = await _botContext.TwitterUsers.FirstOrDefaultAsync(x =>
                    x.ScreenName.Equals(message.Content, StringComparison.CurrentCultureIgnoreCase));
                if (twitter is null)
                {
                    await ReplyAsync("Please setup that user with !twitter add");
                }
                else
                {
                    owner.TwitterUserId = twitter.Id;
                }
            }

            await ReplyAsync("Enter the name for the default Youtube account to follow");
            message = await NextMessageAsync();
            if (!(message is null))
            {
                var youtube = await _botContext.Youtubers.FirstOrDefaultAsync(x =>
                    x.Name.Equals(message.Content, StringComparison.CurrentCultureIgnoreCase));
                if (youtube is null)
                {
                    await ReplyAsync("Please setup that user with !youtube add");
                }
                else
                {
                    owner.YoutuberId = youtube.Id;
                }
            }

            await ReplyAsync("Enter the name for the default Twitch account to follow");
            message = await NextMessageAsync();
            if (!(message is null))
            {
                var twitch = await _botContext.TwitchStreamers.FirstOrDefaultAsync(x =>
                    x.Name.Equals(message.Content, StringComparison.CurrentCultureIgnoreCase));
                if (twitch is null)
                {
                    await ReplyAsync("Please setup that user with !twitch add");
                }
                else
                {
                    owner.TwitchStreamerId = twitch.Id;
                }
            }


            _botContext.GuildOwners.Add(owner);

            var changes = await _botContext.SaveChangesAsync();

            if (changes > 0)
            {
                await ReplyAsync("Guild Setup Successfully");
                await ReplyAsync(
                    "Run `!guild twitter`, `!guild youtube`, `!guild twitch` to set default accounts if you need to");
                return;
            }

            await ReplyAsync("Unable to save changes try again or get Thing");
        }

        [RequireOwnerOrAdmin]
        [Command("twitter", RunMode = RunMode.Async)]
        [Summary("Set the default twitter for guild")]
        [Remarks("?guild twitter omnidestiny")]
        public async Task DefaultTwitter(string twitterName)
        {
            var twitter = await _botContext.TwitterUsers.FirstOrDefaultAsync(x =>
                x.ScreenName.Equals(twitterName, StringComparison.CurrentCultureIgnoreCase));
            if (twitter is null)
            {
                await ReplyAsync("Please setup that user with !twitter add");
            }
            else
            {
                var owner = await _botContext.GuildOwners.FindAsync((long) Context.Guild.OwnerId,
                    (long) Context.Guild.Id);

                if (owner is null)
                {
                    await ReplyAsync("Please run `!guild setup` first");
                    return;
                }

                owner.TwitterUserId = twitter.Id;
                await SaveOwner();
            }
        }

        [RequireOwnerOrAdmin]
        [Command("youtube", RunMode = RunMode.Async)]
        [Summary("Set default Youtube channel for guild")]
        [Remarks("?guild youtube destiny")]
        public async Task DefaultYoutube(string youtubeName)
        {
            var youtube = await _botContext.Youtubers.FirstOrDefaultAsync(x =>
                x.Name.Equals(youtubeName, StringComparison.CurrentCultureIgnoreCase));
            if (youtube is null)
            {
                await ReplyAsync("Please setup that user with !youtube add");
            }
            else
            {
                var owner = await _botContext.GuildOwners.FindAsync((long) Context.Guild.OwnerId,
                    (long) Context.Guild.Id);

                if (owner is null)
                {
                    await ReplyAsync("Please run `!guild setup` first");
                    return;
                }

                owner.YoutuberId = youtube.Id;
                await SaveOwner();
            }
        }

        [RequireOwnerOrAdmin]
        [Command("twitch", RunMode = RunMode.Async)]
        [Summary("Set default twitch streamer for guild")]
        [Remarks("?guild twitch destiny")]
        public async Task DefaultTwitch(string twitchName)
        {
            var twitch = await _botContext.TwitchStreamers.FirstOrDefaultAsync(x =>
                x.Name.Equals(twitchName, StringComparison.CurrentCultureIgnoreCase));
            if (twitch is null)
            {
                await ReplyAsync("Please setup that user with !twtich add");
            }
            else
            {
                var owner = await _botContext.GuildOwners.FindAsync((long) Context.Guild.OwnerId,
                    (long) Context.Guild.Id);

                if (owner is null)
                {
                    await ReplyAsync("Please run `!guild setup` first");
                    return;
                }

                owner.TwitchStreamerId = twitch.Id;
                await SaveOwner();
            }
        }

        private async Task SaveOwner()
        {
            var changes = await _botContext.SaveChangesAsync();
            if (changes > 0)
            {
                await ReplyAsync($"Saved Default Account");
            }
            else
            {
                await ReplyAsync($"Unable to save account");
            }
        }

        protected override void AfterExecute(CommandInfo command)
        {
            base.AfterExecute(command);
            _botContext.Dispose();
        }
    }
}