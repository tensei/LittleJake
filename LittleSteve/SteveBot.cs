using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FluentScheduler;
using LittleSteve.Data;
using LittleSteve.Models;
using LittleSteve.Services;
using LittleSteve.Services.Twitter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace LittleSteve
{
    public class SteveBot
    {
        private readonly DiscordSocketClient _client;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _services;

        public SteveBot()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
#if DEBUG
                LogLevel = LogSeverity.Verbose,
#else
                LogLevel = LogSeverity.Info,
#endif
                DefaultRetryMode = RetryMode.AlwaysRetry
            });

            _config = BuildConfig();
            _services = ConfigureServices();
            SetupJobs();

            Log.Information("Data {@data}", _config.Get<BotConfig>());
        }

        private void SetupJobs()
        {
            var registry = new Registry();
            registry.Schedule(() => new TwitterMonitoringJob(962385627663695872,
                new TwitterService(_services.GetRequiredService<IOptions<BotConfig>>().Value.TwitterTokens),
                _services.GetService<SteveBotContext>())).WithName("test").ToRunEvery(15).Seconds();
            //  JobManager.JobStart += info => Log.Information(info.Name); 
            JobManager.Initialize(registry);
        }

        public async Task StartAsync()
        {
            _client.Log += BotLogHook.Log;
            _client.Ready += async () => { await _client.SetGameAsync("Deathmatch with Wander"); };
            await _client.LoginAsync(TokenType.Bot, _config.Get<BotConfig>().DiscordToken);

            await _client.StartAsync();

            await _services.GetRequiredService<CommandHandlingService>().InitializeAsync(_services);
            await Task.Delay(-1);
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton(new TwitterService(_config.Get<BotConfig>().TwitterTokens))
                .Configure<BotConfig>(_config)
                //We delegate the config object so we dont have to use IOptionsSnapshot or IOptions in our code
                .AddScoped(provider => provider.GetRequiredService<IOptions<BotConfig>>().Value)
                .AddOptions()
                .AddDbContext<SteveBotContext>(opt => opt.UseNpgsql(_config.Get<BotConfig>().ConnectionString),
                    ServiceLifetime.Transient)
                .BuildServiceProvider();
        }

        private IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("config.json", false, true)
                .Build();
        }
    }
}