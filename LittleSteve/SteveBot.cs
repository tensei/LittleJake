using System;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using FluentScheduler;
using LittleSteve.Data;
using LittleSteve.Jobs;
using LittleSteve.Models;
using LittleSteve.Services;
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
                MessageCacheSize = 100,
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
             
            
            Log.Information("Data {@data}", _config.Get<BotConfig>());
        }

        private void SetupJobs()
        {
            // These jobs wont scale well
            // But only one guild is using it and this implementation is meets my needs.
            var registry = new Registry();

            registry.NonReentrantAsDefault();

            using (var context = _services.GetService<SteveBotContext>())
            {
                foreach (var user in context.TwitterUsers)
                {
                    registry.Schedule(() => new TwitterMonitoringJob(user.Id, _services.GetService<TwitterService>(),
                            _services.GetService<SteveBotContext>(), _services.GetService<DiscordSocketClient>())).WithName(user.ScreenName).ToRunNow()
                        .AndEvery(60).Seconds();
                }

                foreach (var streamer in context.TwitchStreamers)
                {
                    registry.Schedule(() => new TwitchMonitoringJob(streamer.Id, _services.GetService<TwitchService>(),
                            _services.GetService<SteveBotContext>(),_services.GetService<DiscordSocketClient>())).WithName(streamer.Name).ToRunNow()
                        .AndEvery(60).Seconds();
                }

                foreach (var youtuber in context.Youtubers)
                {
                    registry.Schedule(() =>
                            new YoutubeMonitoringJob(youtuber.Id, _services.GetService<SteveBotContext>(), _services.GetService<DiscordSocketClient>()))
                        .WithName(youtuber.Id).ToRunNow().AndEvery(5).Minutes();
                }
            }


            JobManager.Initialize(registry);
        }

        public async Task StartAsync()
        {
           
        
            _client.Log += BotLogHook.Log;
            _client.Ready += async () =>
            {
                await _client.SetGameAsync("Deathmatch with Wander");
                SetupJobs();

            };
          
            await _client.LoginAsync(TokenType.Bot, _config.Get<BotConfig>().DiscordToken);

            await _client.StartAsync();

            await _services.GetRequiredService<CommandHandlingService>().InitializeAsync(_services);
     
            await Task.Delay(-1);
        }

      

        private IServiceProvider ConfigureServices()
        {
            var config = _config.Get<BotConfig>();
            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
           
                .AddSingleton(new ImgurService(config.ImgurClientId))
                .AddSingleton(new TwitterService(config.TwitterTokens))
                .AddSingleton(new TwitchService(config.TwitchClientId))
                .AddSingleton<FerretService>()
                .AddSingleton<InteractiveService>()
               .AddSingleton<HttpClient>()
                .Configure<BotConfig>(_config)

                //We delegate the config object so we dont have to use IOptionsSnapshot or IOptions in our code
                .AddTransient(provider => provider.GetRequiredService<IOptionsMonitor<BotConfig>>().CurrentValue)
                .AddOptions()
                .AddDbContext<SteveBotContext>(opt => opt.UseNpgsql(config.ConnectionString),
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