using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FluentScheduler;
using LittleSteve.Preconditions;

namespace LittleSteve.Modules
{
    [Name("Utility")]
    public class UtilityModule : ModuleBase<JakeBotCommandContext>
    {
        [Command("latency")]
        [RequireOwnerOrAdmin]
        [Summary("Show bot gateway and api latency")]
        public async Task PingAsync()
        {
            var watch = Stopwatch.StartNew();
            await ReplyAsync($"Gateway Latency: {Context.Client.Latency}ms");

            watch.Stop();
            await ReplyAsync($"Api Latency: {watch.ElapsedMilliseconds}ms");
        }

        [Command("jobs")]
        [RequireOwnerOrAdmin]
        [Summary("Show the current long running jobs")]
        public async Task JobsAsync()
        {
            var embed = new EmbedBuilder();
            if (!JobManager.AllSchedules.Any())
            {
                await ReplyAsync("No Jobs Running");
                return;
            }

            embed.Title = "Currenly Running Jobs";
            foreach (var schedule in JobManager.AllSchedules)
            {
                embed.AddField(schedule.Name, $"Next Run: {schedule.NextRun:G}");
            }

            await ReplyAsync("", embed: embed.Build());
        }

        // [Command("bot")]
        // [Alias("steve")]
        // [Summary("Bot stuff")]
        // public async Task Bot()
        // {
        //     var builder = new StringBuilder()

        //         .AppendLine("I hate you all")
        //         .AppendLine("If you want the bot to do something let me know")
        //         .AppendLine("Or even better open up an issue on Github detailing your wanted functionality")
        //         .AppendLine("https://github.com/niceprogramming/LittleSteve")

        //         .AppendLine("From, Tensei#0001");

        //     await ReplyAsync(builder.ToString());
        // }


    }
}