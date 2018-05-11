using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace LittleJake.Preconditions
{
    public class RequireOwnerOrAdminAttribute : PreconditionAttribute
    {
        //todo make this search id instead of name
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command,
            IServiceProvider services)
        {
            var user = context.User as IGuildUser;
            if (user.GuildPermissions.Administrator ||
                (await context.Client.GetApplicationInfoAsync()).Owner.Id == user.Id)
            {
                return PreconditionResult.FromSuccess();
            }

            return PreconditionResult.FromError("");
        }
    }
}