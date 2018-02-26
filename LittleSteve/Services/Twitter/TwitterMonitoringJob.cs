using System.Linq;
using FluentScheduler;
using LittleSteve.Data;
using LittleSteve.Extensions;
using Serilog;

namespace LittleSteve.Services.Twitter
{
    public class TwitterMonitoringJob : IJob
    {
        private readonly SteveBotContext _botContext;
        private readonly TwitterService _twitterService;
        private readonly long _twitterUserId;

        public TwitterMonitoringJob(long twitterUserId, TwitterService twitterService, SteveBotContext botContext)
        {
            _twitterUserId = twitterUserId;
            _twitterService = twitterService;
            _botContext = botContext;
        }

        public void Execute()
        {
            using (_botContext)
            {
                var user = _botContext.TwitterUsers.FirstOrDefault(x => x.Id == _twitterUserId);
                if (user is null)
                {
                    return;
                }

                if (user.LastTweetId == 0)
                {
                    var tweet = _twitterService.GetLatestTweetForUserAsync(_twitterUserId).AsSync(false);
                    Log.Information("{date}: {tweet}",tweet.CreatedAt,tweet.FullText ?? tweet.Text);
                    user.LastTweetId = tweet.Id;
                }
                else
                {

                    var tweets = _twitterService.GetTweetsSinceAsync(user.Id, user.LastTweetId).AsSync(false).ToList();

                    if (!tweets.Any())
                    {
                        return;
                    }
                    foreach (var tweet in tweets)
                    {
                        Log.Information("{date}: {tweet}", tweet.CreatedAt, tweet.FullText);
                    }

               
                    user.LastTweetId = tweets.Last().Id;
                }
              
                _botContext.SaveChanges();
            }
        }
    }
}