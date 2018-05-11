using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using LittleJake.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LittleJake.Preconditions
{
    public class BlacklistAttribute : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (await services.GetService<BlacklistService>().IsBlackListed((long) context.User.Id,(long) context.Guild.Id))
            {
                return PreconditionResult.FromError("User blacklisted");
            }
            return PreconditionResult.FromSuccess();
        }
    }
}
