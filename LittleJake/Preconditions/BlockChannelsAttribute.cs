using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace LittleJake.Preconditions
{
    public class BlockChannelsAttribute : PreconditionAttribute
    {
        private readonly ulong[] _channelIds;

        public BlockChannelsAttribute(params ulong[] channelIds)
        {
            _channelIds = channelIds;
        }

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            // just to make the "error" go away
            await Task.Delay(1);
            if (_channelIds.Contains(context.Channel.Id))
            {
                return PreconditionResult.FromError("Channel is blocked");
            }

            return PreconditionResult.FromSuccess();
        }
    }
}
