﻿using System;
using Discord.Commands;
using Discord.WebSocket;
using LittleSteve.Data;
using LittleSteve.Data.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace LittleSteve
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