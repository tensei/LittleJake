using System.Linq;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Models.API.v5.Streams;
using TwitchLib.Models.API.v5.Users;

namespace LittleSteve.Services
{
    public class TwitchService
    {
        private readonly TwitchAPI _api;

        public TwitchService(string clientId)
        {
            _api = new TwitchAPI(clientId);
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            var users = await _api.Users.v5.GetUserByNameAsync(name);

            return users.Matches.FirstOrDefault();
        }

        public Task<User> GetUserByIdAsync(long channelId) => _api.Users.v5.GetUserByIDAsync(channelId.ToString());

        public Task<bool> IsUserStreamingAsync(long channelId) =>
            _api.Streams.v5.BroadcasterOnlineAsync(channelId.ToString());

        public async Task<Stream> GetStreamAsync(long channelId) =>
            (await _api.Streams.v5.GetStreamByUserAsync(channelId.ToString())).Stream;
    }
}