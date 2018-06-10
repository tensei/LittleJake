using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Api.Models.v5.Streams;
using TwitchLib.Api.Models.v5.Users;

namespace LittleJake.Services
{
    public class TwitchService
    {
        private readonly TwitchAPI _api;

        public TwitchService(string clientId)
        {
            _api = new TwitchAPI();
            _api.Settings.ClientId = clientId;
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            var users = await _api.Users.v5.GetUserByNameAsync(name);

            return users.Matches.FirstOrDefault();
        }

        public Task<User> GetUserByIdAsync(long channelId) => _api.Users.v5.GetUserByIDAsync(channelId.ToString());

        public async Task<bool> IsUserStreamingAsync(long channelId)
        {
            var stream = await _api.Streams.v5.GetStreamByUserAsync(channelId.ToString(), "live");
            return stream.Stream != null;
        }


        public async Task<Stream> GetStreamAsync(long channelId) =>
            (await _api.Streams.v5.GetStreamByUserAsync(channelId.ToString())).Stream;
    }
}