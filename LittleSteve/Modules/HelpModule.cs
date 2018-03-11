using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LittleSteve;
using LittleSteve.Extensions;

namespace LittleSteve.Modules
{
    //https://github.com/AntiTcb/DiscordBots/blob/vs17-convert/src/DiscordBCL/Modules/InfoModule.cs
 
    [Name("Help")]
    public class HelpModule : ModuleBase<SteveBotCommandContext>
    {
        private readonly IServiceProvider _provider;
        public CommandService CommandService { get; set; }

        public HelpModule(IServiceProvider provider)
        {
            _provider = provider;
        }

        [Command("help", RunMode = RunMode.Async)]
        [Alias("commands")]
        [Remarks("help")]
        [Summary("List available commands")]
        public async Task HelpAsync()
        {
            var modules = CommandService.Modules
                .Where(m => m.CanExecute(Context, _provider) )
                .OrderBy(m => m.Name);

            var sentMessage = await ReplyAsync("", embed: modules.GetEmbed(Context, _provider)).ConfigureAwait(false);
            await Task.Delay(30000).ConfigureAwait(false);
            await sentMessage.DeleteAsync().ConfigureAwait(false);
        }

        [Command("help", RunMode = RunMode.Async), Priority(1)]
        [Alias("help:command")]
        [Summary("Information about a specific command.")]
        [Remarks("help prefix")]
        public async Task HelpAsync([Remainder]CommandInfo commandName)
        {
            if (!commandName.CanExecute(Context, _provider))
                await ReplyAsync("You do not have permission to run this command.").ConfigureAwait(false);
            else
                await ReplyAsync("", embed: commandName.GetEmbed(Context)).ConfigureAwait(false);
        }

        [Command("help", RunMode = RunMode.Async), Priority(2)]
        [Alias("help:module")]
        [Summary("Information about a specific module.")]
        [Remarks("help config")]
        public async Task HelpAsync([Remainder]ModuleInfo moduleName)
        {
            if (!moduleName.CanExecute(Context, _provider))
                await ReplyAsync("You do not have permission to view this module.").ConfigureAwait(false);
            else
                await ReplyAsync("", embed: moduleName.GetEmbed(Context, _provider)).ConfigureAwait(false);
        }
    }
}
