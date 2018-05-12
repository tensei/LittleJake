
using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using LittleJake.Data;
using Serilog;

namespace LittleJake.Services
{
    public class UserService
    {
        private DiscordSocketClient _client;
        private readonly JakeBotContext _botContext;

        public UserService(DiscordSocketClient client, JakeBotContext botContext)
        {
            _client = client;
            _botContext = botContext;
            _client.UserJoined += UserJoined;
            _client.UserLeft += UserLeft;
        }

        private Task UserLeft(SocketGuildUser user)
        {
            // TODO: add more stuff
            Log.Information($"User: {user} left");
            return Task.CompletedTask;
        }

        private Task UserJoined(SocketGuildUser user)
        {
            // TODO: add more stuff
            Log.Information($"User: {user} joined");
            return Task.CompletedTask;
        }
    }
}