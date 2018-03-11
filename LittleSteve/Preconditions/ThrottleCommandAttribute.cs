using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LittleSteve.Models;
using LittleSteve.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LittleSteve.Preconditions
{
    public class ThrottleCommandAttribute : PreconditionAttribute
    {
        
        private readonly Dictionary<(ulong, string), DateTimeOffset> _throttles = new Dictionary<(ulong, string), DateTimeOffset>();

        
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var expiryPeriod = TimeSpan.FromSeconds(services.GetService<BotConfig>().ThrottleLength);
            if ((await context.Client.GetApplicationInfoAsync()).Owner.Username == context.User.Username)
            {
                return PreconditionResult.FromSuccess();
            }

            var user = context.User as IGuildUser;

            if (user.GuildPermissions.Administrator)
            {
                return PreconditionResult.FromSuccess();
            }
            
            var key = (context.Channel.Id, command.Name);
            var keyExist = _throttles.TryGetValue(key, out var time);
            if (!keyExist || DateTimeOffset.UtcNow > time)
            {
                _throttles[key] = DateTimeOffset.UtcNow + expiryPeriod;
                return PreconditionResult.FromSuccess();
            }
           

            Console.WriteLine("throttled");
            return PreconditionResult.FromError(string.Empty);
        }
    }
}
