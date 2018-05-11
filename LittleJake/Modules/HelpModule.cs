using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using LittleJake.Extensions;
using LittleJake.Preconditions;

namespace LittleJake.Modules
{
    //https://github.com/AntiTcb/DiscordBots/blob/vs17-convert/src/DiscordBCL/Modules/InfoModule.cs

    [Name("Help")]
    public class HelpModule : ModuleBase<JakeBotCommandContext>
    {
        private readonly IServiceProvider _provider;

        public HelpModule(IServiceProvider provider)
        {
            _provider = provider;
        }

        public CommandService CommandService { get; set; }

        [Command("help", RunMode = RunMode.Async)]
        [Alias("commands")]
        [Remarks("?help")]
        [Summary("List available commands")]
        [ThrottleCommand]
        public async Task HelpAsync()
        {
            var modules = CommandService.Modules
                .Where(m => m.CanExecute(Context, _provider))
                .OrderBy(m => m.Name);

            var sentMessage = await ReplyAsync("", embed: modules.GetEmbed(Context, _provider)).ConfigureAwait(false);
            await Task.Delay(60000).ConfigureAwait(false);
            await sentMessage.DeleteAsync().ConfigureAwait(false);
        }

        [Command("help", RunMode = RunMode.Async)]
        [Priority(1)]
        [Alias("help:command", "help:c")]
        [Summary("Information about a specific command.")]
        [Remarks("?help:c aslan")]
        public async Task HelpAsync([Remainder] CommandInfo commandName)
        {
            if (!commandName.CanExecute(Context, _provider))
            {
                await ReplyAsync("You do not have permission to run this command.").ConfigureAwait(false);
            }
            else
            {
                await ReplyAsync("", embed: commandName.GetEmbed(Context)).ConfigureAwait(false);
            }
        }

        [Command("help", RunMode = RunMode.Async)]
        [Priority(2)]
        [Alias("help:module", "help:m")]
        [Summary("Information about a specific module.")]
        [Remarks("?help:m aslan")]
        public async Task HelpAsync([Remainder] ModuleInfo moduleName)
        {
            if (!moduleName.CanExecute(Context, _provider))
            {
                await ReplyAsync("You do not have permission to view this module.").ConfigureAwait(false);
            }
            else
            {
                await ReplyAsync("", embed: moduleName.GetEmbed(Context, _provider)).ConfigureAwait(false);
            }
        }
    }
}