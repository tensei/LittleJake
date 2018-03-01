using System;
using Discord.Commands;
using Discord.WebSocket;

namespace LittleSteve
{
    public class SteveBotCommandContext : SocketCommandContext
    {
        public SteveBotCommandContext(DiscordSocketClient client, SocketUserMessage msg, IServiceProvider provider) :
            base(client, msg)
        {
            Provider = provider;
        }

        public IServiceProvider Provider { get; }
    }
}