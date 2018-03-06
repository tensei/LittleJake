using System;
using System.Linq;
using Discord.Commands;
using Discord.WebSocket;
using LittleSteve.Data;
using LittleSteve.Data.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace LittleSteve
{
    public class SteveBotCommandContext : SocketCommandContext
    {
        public SteveBotCommandContext(DiscordSocketClient client, SocketUserMessage msg, IServiceProvider provider) :
            base(client, msg)
        {
            Provider = provider;
            GuildOwner = Provider.GetService<SteveBotContext>().Find<GuildOwner>((long)base.Guild.OwnerId,(long) base.Guild.Id);

        }

        public IServiceProvider Provider { get; }
        public GuildOwner GuildOwner { get; }
    }
}