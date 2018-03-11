using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FluentScheduler;
using LittleSteve.Preconditions;

namespace LittleSteve.Modules
{
    [Name("Utility")]
    public class UtilityModule : ModuleBase<SteveBotCommandContext>
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

        [Command("test")]
        [RequireOwnerOrAdmin]
        public async Task Test()
        {
        }
    }
}