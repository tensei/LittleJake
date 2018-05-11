﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreTweet;
using LittleJake.Models;

namespace LittleJake.Services
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

        public async Task<Status> GetLatestTweetForUserAsync(long userId) =>
            (await _token.Statuses.UserTimelineAsync(userId, 1, exclude_replies: true, include_rts: false)).Single();

        public async Task<Status> GetTweetByIdAsync(long tweetId)
        {
            var tweets = await _token.Statuses.LookupAsync(new List<long> { tweetId }, tweet_mode: TweetMode.Extended);
            return tweets.FirstOrDefault();
        }

        public async Task<IEnumerable<Status>> GetTweetsSinceAsync(long userId, long tweetId, int count = 50)
        {
            var tweets =
                await _token.Statuses.UserTimelineAsync(userId, count, tweetId, exclude_replies: true, include_rts: false, tweet_mode: TweetMode.Extended);
            return tweets.OrderBy(x => x.CreatedAt);
        }

        public async Task<UserResponse> GetUserFromHandle(string handle)
        {
            try
            {
                return await _token.Users.ShowAsync(handle);
            }
            catch (TwitterException)
            {
                return null;
            }
        }
    }
}