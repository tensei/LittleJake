using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LittleSteve.Models;
using LittleSteve.TypeReaders;
using Serilog;

namespace LittleSteve.Services
{
    public class CommandHandlingService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly BotConfig _config;
        private IServiceProvider _provider;

        public CommandHandlingService(DiscordSocketClient client, CommandService commands, BotConfig config)
        {
            _client = client;
            _commands = commands;
            _config = config;
            _commands.Log += BotLogHook.Log;

            _client.MessageReceived += MessageReceived;
        }

        public async Task InitializeAsync(IServiceProvider provider)
        {
            _provider = provider;
            _commands.AddTypeReader<ModuleInfo>(new ModuleInfoTypeReader());
            _commands.AddTypeReader<CommandInfo>(new CommandInfoTypeReader());

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());

        }

        private async Task MessageReceived(SocketMessage rawMessage)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message))
            {
                return;
            }

            if (message.Source != MessageSource.User)
            {
                return;
            }


            var argPos = 0;
            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) ||
                  message.HasStringPrefix(_config.Prefix, ref argPos)))
            {
                return;
            }

            var context = new JakeBotCommandContext(_client, message, _provider);

            var result = await _commands.ExecuteAsync(context, argPos, _provider);

            if (!result.IsSuccess)
            {
                Log.Error($"{result.Error}: {result.ErrorReason}");
            }

            stopwatch.Stop();
            Log.Information($"Took {stopwatch.ElapsedMilliseconds}ms to process: {message.Author} {message}");
        }
    }
}