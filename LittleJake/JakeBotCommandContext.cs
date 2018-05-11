using System;
using Discord.Commands;
using Discord.WebSocket;
using LittleJake.Data;
using LittleJake.Data.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace LittleJake
{
    public class JakeBotCommandContext : SocketCommandContext
    {
        public JakeBotCommandContext(DiscordSocketClient client, SocketUserMessage msg, IServiceProvider provider) :
            base(client, msg)
        {
            GuildOwner = provider.GetService<JakeBotContext>().Find<GuildOwner>((long)Guild.OwnerId, (long)Guild.Id);
        }

        public GuildOwner GuildOwner { get; }
    }
}