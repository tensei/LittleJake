using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using LittleJake.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace LittleJake.TypeReaders
{
    public class CommandInfoTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input,
            IServiceProvider services)
        {
            var cmdService = services.GetRequiredService<CommandService>();
            var cmd = cmdService.Commands.FirstOrDefault(c => c.Aliases.Any(a =>
                string.Equals(a, input, StringComparison.OrdinalIgnoreCase) && c.CanExecute(context, services)));

            if (cmd == null)
            {
                return Task.FromResult(
                    TypeReaderResult.FromError(CommandError.ObjectNotFound, "Command was not found."));
            }

            return Task.FromResult(TypeReaderResult.FromSuccess(cmd));
        }
    }
}