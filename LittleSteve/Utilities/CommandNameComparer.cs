using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace LittleSteve.Utilities
{
    internal class CommandNameComparer : IEqualityComparer<CommandInfo>
    {
        public bool Equals(CommandInfo x, CommandInfo y) => x.Name.Equals(y.Name, StringComparison.OrdinalIgnoreCase) && x.Module.Name != y.Module.Name;
        public int GetHashCode(CommandInfo obj) => obj.Name.ToLowerInvariant().GetHashCode();
    }
}
