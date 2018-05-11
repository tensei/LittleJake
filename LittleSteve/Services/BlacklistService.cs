using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using LittleSteve.Data;
using LittleSteve.Data.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace LittleSteve.Services
{
    public class BlacklistService
    {
        private readonly IServiceProvider _services;

        public BlacklistService(IServiceProvider services)
        {
            _services = services;
        }

        public async Task<UserBlacklist> Add(long userId, long guildId)
        {
            using (var context = _services.GetService<JakeBotContext>())
            {
                var blacklist = new UserBlacklist() { GuildId = guildId, UserId = userId };
                if (await IsBlackListed(userId, guildId))
                {
                    return blacklist;
                }

                blacklist = new UserBlacklist() { GuildId = guildId, UserId = userId };
                context.Add(blacklist);
                var changes = await context.SaveChangesAsync();
                return changes > 0 ? blacklist : null;
            }
        }

        public async Task<UserBlacklist> Remove(long userId, long guildId)
        {
            using (var context = _services.GetService<JakeBotContext>())
            {
                var blacklist = new UserBlacklist() { GuildId = guildId, UserId = userId };
                if (await IsBlackListed(userId, guildId))
                {
                    context.UserBlacklists.Remove(blacklist);
                    return await context.SaveChangesAsync() > 0 ? blacklist : null;
                }

                return blacklist;
            }
        }
        public async Task<bool> IsBlackListed(long userId, long guildId)
        {
            using (var context = _services.GetService<JakeBotContext>())
            {
                return await context.UserBlacklists.FindAsync(guildId, userId) != null;
            }
        }



    }
}
