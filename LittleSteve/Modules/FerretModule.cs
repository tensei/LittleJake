using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using LittleSteve.Services;
using Microsoft.EntityFrameworkCore;

namespace LittleSteve.Modules
{
    public class FerretModule : ModuleBase<SteveBotCommandContext>
    {
        private readonly FerretService _ferretService;
        private readonly HttpClient _httpClient;

        public FerretModule(FerretService ferretService ,HttpClient httpClient)
        {
            _ferretService = ferretService;
            _httpClient = httpClient;
        }
        [Command("ferret",RunMode = RunMode.Async)]
        public async Task Ferret([Remainder] string question = null)
        {
            var picture = await _ferretService.GetFerretPicture();

           var stream = await _httpClient.GetStreamAsync(picture.Url);

           await Context.Channel.SendFileAsync(stream, picture.Url.Split('/').Last());
        }
    }
}
