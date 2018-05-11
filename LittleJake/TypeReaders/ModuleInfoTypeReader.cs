using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using LittleJake.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace LittleJake.TypeReaders
{
    public class ModuleInfoTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input,
            IServiceProvider services)
        {
            var cmdService = services.GetRequiredService<CommandService>();
            var module = cmdService.Modules.FirstOrDefault(m =>
                string.Equals(m.Name, input, StringComparison.OrdinalIgnoreCase) && m.CanExecute(context, services));

            if (module == null)
            {
                return Task.FromResult(TypeReaderResult.FromError(CommandError.ObjectNotFound,
                    "Module was not found."));
            }

            return Task.FromResult(TypeReaderResult.FromSuccess(module));
        }
    }
}