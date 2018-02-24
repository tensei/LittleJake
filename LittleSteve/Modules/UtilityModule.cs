using System.Diagnostics;
using System.Threading.Tasks;
using Discord.Commands;

namespace LittleSteve.Modules
{
    public class UtilityModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            var watch = Stopwatch.StartNew();
            await ReplyAsync($"Gateway Latency: {Context.Client.Latency}ms");

            watch.Stop();
            await ReplyAsync($"Api Latency: {watch.ElapsedMilliseconds}ms");
        }
    }
}