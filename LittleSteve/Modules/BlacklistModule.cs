using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LittleSteve.Preconditions;
using LittleSteve.Services;

namespace LittleSteve.Modules
{
    [Group("blacklist")]
    public class BlacklistModule : ModuleBase<SteveBotCommandContext>
    {
        private readonly BlacklistService _blacklistService;

        public BlacklistModule(BlacklistService blacklistService)
        {
            _blacklistService = blacklistService;
        }
        [Command("add")]
        [RequireOwnerOrAdmin]
        public async Task Add(IGuildUser guildUser)
        {
           var user = await _blacklistService.Add((long) guildUser.Id, (long) Context.Guild.Id);
            if (user is null)
            {
                await ReplyAsync("Unable to blacklist this user");
            }
            else
            {
                await ReplyAsync("User Blacklisted");
            }
        }

        [Command("remove")]
        [RequireOwnerOrAdmin]
        public async Task Remove(IGuildUser guildUser)
        {
            var user = await _blacklistService.Remove((long)guildUser.Id, (long)Context.Guild.Id);
            if (user is null)
            {
                await ReplyAsync("Unable to remove user from blacklist");
            }
            else
            {
                await ReplyAsync("User removed from blacklist");
            }
        }
    }
}
