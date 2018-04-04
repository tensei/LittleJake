using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord.Commands;
using LittleSteve.Preconditions;
using LittleSteve.Services;

namespace LittleSteve.Modules
{
    [Name("Ferret")]
    public class FerretModule : ModuleBase<SteveBotCommandContext>
    {
        private readonly FerretService _ferretService;
        private readonly HttpClient _httpClient;

        public FerretModule(FerretService ferretService, HttpClient httpClient)
        {
            _ferretService = ferretService;
            _httpClient = httpClient;
        }

        [Command("ferret", RunMode = RunMode.Async)]
        [Summary("Get a picture of a ferret")]
        [Blacklist]
        [BlockChannels()]
        [ThrottleCommand]
        [Remarks("?ferret what should i do today")]
        public async Task Ferret([Remainder] string question = null)
        {
            var picture = await _ferretService.GetFerretPicture();

            var stream = await _httpClient.GetStreamAsync(picture.Url);

            await Context.Channel.SendFileAsync(stream, picture.Url.Split('/').Last());
        }
    }
}