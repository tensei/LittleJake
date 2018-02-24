using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreTweet;
using LittleSteve.Models;

namespace LittleSteve.Services.Twitter
{
    public class TwitterService
    {
        private readonly Tokens _token;


        public TwitterService(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            _token = Tokens.Create(consumerKey, consumerSecret, accessToken, accessTokenSecret);
        }

        public TwitterService(TwitterTokens tokens) : this(tokens.ConsumerKey, tokens.ConsumerSecret,
            tokens.AccessToken, tokens.AccessTokenSecret)
        {
        }

        public async Task<Status> GetLatestTweetForUser(long userId)
        {
            return (await _token.Statuses.UserTimelineAsync(userId, 1)).Single();
        }

        public async Task<Status> GetTweetById(long tweetId)
        {
            var tweets = await _token.Statuses.LookupAsync(new List<long> {tweetId}, tweet_mode: TweetMode.Extended);
            return tweets.FirstOrDefault();
        }

        public async Task<IEnumerable<Status>> GetTweetSince(long userId, long tweetId)
        {
            var tweets = await _token.Statuses.UserTimelineAsync(userId, since_id: tweetId);
            return tweets.Where(x => x.IsRetweeted ?? false).OrderBy(x => x.CreatedAt);
        }
    }
}