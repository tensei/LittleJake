using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace LittleSteve
{
    public class SteveBotCommandContext : SocketCommandContext
    {
        public IServiceProvider Provider { get; }

        public SteveBotCommandContext(DiscordSocketClient client, SocketUserMessage msg,IServiceProvider provider) : base(client, msg)
        {
            Provider = provider;
        }
    }
}
