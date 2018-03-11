using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using LittleSteve.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace LittleSteve.TypeReaders
{
    public class ModuleInfoTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            var cmdService = services.GetRequiredService<CommandService>();
            var module = cmdService.Modules.FirstOrDefault(m =>
                string.Equals(m.Name, input, StringComparison.OrdinalIgnoreCase) && m.CanExecute(context, services));

            if (module == null)
                return Task.FromResult(TypeReaderResult.FromError(CommandError.ObjectNotFound, "Module was not found."));
            else
                return Task.FromResult(TypeReaderResult.FromSuccess(module));
        }
    }
}
