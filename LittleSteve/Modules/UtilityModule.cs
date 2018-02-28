using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FluentScheduler;
using LittleSteve.Preconditions;

namespace LittleSteve.Modules
{
    public class UtilityModule : ModuleBase<SteveBotCommandContext>
    {
    
        [Command("ping")]
        [RequireOwnerOrAdmin]
        public async Task PingAsync()
        {
            var watch = Stopwatch.StartNew();
            await ReplyAsync($"Gateway Latency: {Context.Client.Latency}ms");

            watch.Stop();
            await ReplyAsync($"Api Latency: {watch.ElapsedMilliseconds}ms");
        }

        [Command("jobs")]
        [RequireOwnerOrAdmin]
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
                
                embed.AddField(schedule.Name,$"Next Run: {schedule.NextRun:G}" );
            }

            await ReplyAsync("", embed: embed.Build());
        }

        [Command("embed")]
        public async Task TestEmbed()
        {
            var embed = new EmbedBuilder()
                .WithAuthor("Destiny is streaming. Watch live at https://www.destiny.gg/bigscreen", url: "https://www.destiny.gg/bigscreen")
                .WithTitle("example")
                .WithUrl("https://www.example.com")
                .WithThumbnailUrl("https://static-cdn.jtvnw.net/jtv_user_pictures/destiny-profile_image-951fd53950bc2f8b-300x300.png")
                .WithImageUrl("https://static-cdn.jtvnw.net/previews-ttv/live_user_destiny-1920x1080.jpg?201822801")
                .AddField("Playing","Terraria",true)
                .AddField("Viewers",2002,true)
                .Build();
            await ReplyAsync("", embed: embed);

        }
    }
}